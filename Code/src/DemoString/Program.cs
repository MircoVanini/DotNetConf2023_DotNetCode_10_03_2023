
using BenchmarkDotNet.Attributes; 
using BenchmarkDotNet.Running; 
using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;


public partial class Program
{

    static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

    // The Project Gutenberg eBook of The Adventures of Sherlock Holmes, by Arthur Conan Doyle
    //
    private static readonly string s_haystack = new HttpClient().GetStringAsync("http://aleph.gutenberg.org/1/6/6/1661/1661-0.txt").Result;

    [Benchmark]
    [Arguments("Sherlock")]
    [Arguments("elementary")]
    public int Count(string needle)
    {
        ReadOnlySpan<char> haystack = s_haystack;
        int count = 0, pos;
        while ((pos = haystack.IndexOf(needle, StringComparison.OrdinalIgnoreCase)) >= 0)
        {
            haystack = haystack.Slice(pos + needle.Length);
            count++;
        }
        return count;
    }

    private byte[] _data = new byte[95];
    [Benchmark]
    public bool Contains() => _data.AsSpan().Contains((byte)1);


    private int[] _dataInt = new int[1000];
    [Benchmark]
    public int IndexOf() => _dataInt.AsSpan().IndexOf(42);


    const string Sonnet = """
    Shall I compare thee to a summer's day?
    Thou art more lovely and more temperate:
    Rough winds do shake the darling buds of May, And summer's lease hath all too short a date; Sometime too hot the eye of heaven shines,
    And often is his gold complexion dimm'd;
    And every fair from fair sometime declines,
    By chance or nature's changing course untrimm'd; But thy eternal summer shall not fade,
    Nor lose possession of that fair thou ow'st;
    Nor shall death brag thou wander'st in his shade, When in eternal lines to time thou grow'st:
    So long as men can breathe or eyes can see,
    So long lives this, and this gives life to thee. 
    """;

    private StringBuilder _builder = new StringBuilder(Sonnet);
    [Benchmark]
    public void Replace()
    {
        _builder.Replace('?', '!');
        _builder.Replace('!', '?');
    }
}