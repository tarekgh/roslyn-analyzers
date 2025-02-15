﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using VerifyCS = Test.Utilities.CSharpCodeFixVerifier<
    Microsoft.NetCore.Analyzers.Performance.RecommendCaseInsensitiveStringComparisonAnalyzer,
    Microsoft.NetCore.CSharp.Analyzers.Performance.CSharpRecommendCaseInsensitiveStringComparisonFixer>;

namespace Microsoft.NetCore.Analyzers.Performance.UnitTests
{
    public class RecommendCaseInsensitiveStringComparison_CSharp_Tests : RecommendCaseInsensitiveStringComparison_Base_Tests
    {
        [Theory]
        [MemberData(nameof(DiagnosedAndFixedData))]
        [MemberData(nameof(DiagnosedAndFixedInvertedData))]
        [MemberData(nameof(CSharpDiagnosedAndFixedNamedData))]
        public async Task Diagnostic_Assign(string diagnosedLine, string fixedLine)
        {
            string originalCode = $@"using System;
class C
{{
    void M()
    {{
        string a = ""aBc"";
        string b = ""bc"";
        var result = [|{diagnosedLine}|];
    }}
}}";
            string fixedCode = $@"using System;
class C
{{
    void M()
    {{
        string a = ""aBc"";
        string b = ""bc"";
        var result = {fixedLine};
    }}
}}";
            await VerifyFixCSharpAsync(originalCode, fixedCode);
        }

        [Theory]
        [MemberData(nameof(DiagnosedAndFixedData))]
        [MemberData(nameof(DiagnosedAndFixedInvertedData))]
        [MemberData(nameof(CSharpDiagnosedAndFixedNamedData))]
        public async Task Diagnostic_Return(string diagnosedLine, string fixedLine)
        {
            string originalCode = $@"using System;
class C
{{
    object M()
    {{
        string a = ""aBc"";
        string b = ""bc"";
        return [|{diagnosedLine}|];
    }}
}}";
            string fixedCode = $@"using System;
class C
{{
    object M()
    {{
        string a = ""aBc"";
        string b = ""bc"";
        return {fixedLine};
    }}
}}";
            await VerifyFixCSharpAsync(originalCode, fixedCode);
        }

        [Theory]
        [MemberData(nameof(DiagnosedAndFixedWithEqualsToData))]
        [MemberData(nameof(DiagnosedAndFixedWithEqualsToInvertedData))]
        [MemberData(nameof(CSharpDiagnosedAndFixedWithEqualsToNamedData))]
        public async Task Diagnostic_If(string diagnosedLine, string fixedLine, string equalsTo)
        {
            string originalCode = $@"using System;
class C
{{
    int M()
    {{
        string a = ""aBc"";
        string b = ""bc"";
        if ([|{diagnosedLine}|]{equalsTo})
        {{
            return 5;
        }}
        return 4;
    }}
}}";
            string fixedCode = $@"using System;
class C
{{
    int M()
    {{
        string a = ""aBc"";
        string b = ""bc"";
        if ({fixedLine}{equalsTo})
        {{
            return 5;
        }}
        return 4;
    }}
}}";
            await VerifyFixCSharpAsync(originalCode, fixedCode);
        }

        [Theory]
        [MemberData(nameof(DiagnosedAndFixedData))]
        [MemberData(nameof(DiagnosedAndFixedInvertedData))]
        [MemberData(nameof(CSharpDiagnosedAndFixedNamedData))]
        public async Task Diagnostic_IgnoreResult(string diagnosedLine, string fixedLine)
        {
            string originalCode = $@"using System;
class C
{{
    void M()
    {{
        string a = ""aBc"";
        string b = ""bc"";
        [|{diagnosedLine}|];
    }}
}}";
            string fixedCode = $@"using System;
class C
{{
    void M()
    {{
        string a = ""aBc"";
        string b = ""bc"";
        {fixedLine};
    }}
}}";
            await VerifyFixCSharpAsync(originalCode, fixedCode);
        }

        [Theory]
        [MemberData(nameof(DiagnosedAndFixedStringLiteralsData))]
        [MemberData(nameof(DiagnosedAndFixedStringLiteralsInvertedData))]
        [MemberData(nameof(CSharpDiagnosedAndFixedStringLiteralsNamedData))]
        public async Task Diagnostic_StringLiterals_ReturnExpressionBody(string diagnosedLine, string fixedLine)
        {
            string originalCode = $@"using System;
class C
{{
    object M() => [|{diagnosedLine}|];
}}";
            string fixedCode = $@"using System;
class C
{{
    object M() => {fixedLine};
}}";
            await VerifyFixCSharpAsync(originalCode, fixedCode);
        }

        [Theory]
        [MemberData(nameof(DiagnosedAndFixedStringReturningMethodsData))]
        [MemberData(nameof(DiagnosedAndFixedStringReturningMethodsInvertedData))]
        [MemberData(nameof(CSharpDiagnosedAndFixedStringReturningMethodsNamedData))]
        public async Task Diagnostic_StringReturningMethods_Discard(string diagnosedLine, string fixedLine)
        {
            string originalCode = $@"using System;
class C
{{
    public string GetStringA() => ""aBc"";
    public string GetStringB() => ""CdE"";
    void M()
    {{
        _ = [|{diagnosedLine}|];
    }}
}}";
            string fixedCode = $@"using System;
class C
{{
    public string GetStringA() => ""aBc"";
    public string GetStringB() => ""CdE"";
    void M()
    {{
        _ = {fixedLine};
    }}
}}";
            await VerifyFixCSharpAsync(originalCode, fixedCode);
        }

        [Theory]
        [MemberData(nameof(DiagnosedAndFixedParenthesizedData))]
        [MemberData(nameof(DiagnosedAndFixedParenthesizedInvertedData))]
        [MemberData(nameof(CSharpDiagnosedAndFixedParenthesizedNamedData))]
        [MemberData(nameof(CSharpDiagnosedAndFixedParenthesizedNamedInvertedData))]
        public async Task Diagnostic_Parenthesized_ReturnCastedToString(string diagnosedLine, string fixedLine)
        {
            string originalCode = $@"using System;
class C
{{
    string GetString() => ""aBc"";
    string M()
    {{
        return ([|{diagnosedLine}|]).ToString();
    }}
}}";
            string fixedCode = $@"using System;
class C
{{
    string GetString() => ""aBc"";
    string M()
    {{
        return ({fixedLine}).ToString();
    }}
}}";
            await VerifyFixCSharpAsync(originalCode, fixedCode);
        }

        [Theory]
        [MemberData(nameof(CSharpDiagnosedAndFixedEqualityToEqualsData))]
        public async Task Diagnostic_Equality_To_Equals(string diagnosedLine, string fixedLine)
        {
            string originalCode = $@"using System;
class C
{{
    string GetString() => ""cde"";
    bool M(string a, string b)
    {{
        bool result = [|{diagnosedLine}|];
        if ([|{diagnosedLine}|]) return result;
        return [|{diagnosedLine}|];
    }}
}}";
            string fixedCode = $@"using System;
class C
{{
    string GetString() => ""cde"";
    bool M(string a, string b)
    {{
        bool result = {fixedLine};
        if ({fixedLine}) return result;
        return {fixedLine};
    }}
}}";
            await VerifyFixCSharpAsync(originalCode, fixedCode);
        }

        [Fact]
        public async Task Diagnostic_Equality_To_Equals_Trivia()
        {
            string originalCode = $@"using System;
class C
{{
    bool M(string a, string b)
    {{
        // Trivia
        bool /* Trivia */ result = /* Trivia */ [|a.ToLower() // Trivia
            == /* Trivia */ b.ToLowerInvariant()|] /* Trivia */; // Trivia
        if (/* Trivia */ [|a.ToLowerInvariant() /* Trivia */ != /* Trivia */ b.ToLower()|] /* Trivia */) // Trivia
            return /* Trivia */ [|b /* Trivia */ != /* Trivia */ a.ToLowerInvariant()|] /* Trivia */; // Trivia
        return // Trivia
            [|""abc"" /* Trivia */ == /* Trivia */ a.ToUpperInvariant()|] /* Trivia */; // Trivia
        // Trivia
    }}
}}";
            string fixedCode = $@"using System;
class C
{{
    bool M(string a, string b)
    {{
        // Trivia
        bool /* Trivia */ result = /* Trivia */ a.Equals(b, StringComparison.CurrentCultureIgnoreCase) /* Trivia */; // Trivia
        if (/* Trivia */ !a.Equals(b, StringComparison.CurrentCultureIgnoreCase) /* Trivia */) // Trivia
            return /* Trivia */ !b /* Trivia */ .Equals /* Trivia */ (a, StringComparison.InvariantCultureIgnoreCase) /* Trivia */; // Trivia
        return // Trivia
            ""abc"" /* Trivia */ .Equals /* Trivia */ (a, StringComparison.InvariantCultureIgnoreCase) /* Trivia */; // Trivia
        // Trivia
    }}
}}";
            await VerifyFixCSharpAsync(originalCode, fixedCode);
        }

        [Theory]
        [MemberData(nameof(NoDiagnosticData))]
        [InlineData("\"aBc\".CompareTo(null)")]
        [InlineData("\"aBc\".ToUpperInvariant().CompareTo((object)null)")]
        [InlineData("\"aBc\".CompareTo(value: (object)\"cDe\")")]
        [InlineData("\"aBc\".CompareTo(strB: \"cDe\")")]
        public async Task NoDiagnostic_All(string ignoredLine)
        {
            string originalCode = $@"using System;
class C
{{
    object M()
    {{
        char ch = 'c';
        object obj = 3;
        return {ignoredLine};
    }}
}}";

            await VerifyNoDiagnosticCSharpAsync(originalCode);
        }

        [Theory]
        [MemberData(nameof(DiagnosticNoFixCompareToData))]
        [MemberData(nameof(DiagnosticNoFixCompareToInvertedData))]
        [MemberData(nameof(CSharpDiagnosticNoFixCompareToNamedData))]
        public async Task Diagnostic_NoFix_CompareTo(string diagnosedLine)
        {
            string originalCode = $@"using System;
class C
{{
    string GetStringA() => ""aBc"";
    string GetStringB() => ""cDe"";
    int M()
    {{
        string a = ""AbC"";
        string b = ""CdE"";
        return [|{diagnosedLine}|];
    }}
}}";
            await VerifyDiagnosticOnlyCSharpAsync(originalCode);
        }

        [Theory]
        [MemberData(nameof(CSharpDiagnosticNoFixEqualsData))]
        public async Task Diagnostic_NoFix_Equals(string diagnosedLine)
        {
            string originalCode = $@"using System;
class C
{{
    bool M()
    {{
        return [|{diagnosedLine}|];
    }}
}}";

            await VerifyDiagnosticOnlyCSharpAsync(originalCode);
        }

        private async Task VerifyNoDiagnosticCSharpAsync(string originalSource)
        {
            VerifyCS.Test test = new()
            {
                TestCode = originalSource,
                FixedCode = originalSource
            };

            await test.RunAsync();
        }

        private async Task VerifyDiagnosticOnlyCSharpAsync(string originalSource)
        {
            VerifyCS.Test test = new()
            {
                TestCode = originalSource,
                MarkupOptions = MarkupOptions.UseFirstDescriptor
            };

            await test.RunAsync();
        }

        private async Task VerifyFixCSharpAsync(string originalSource, string fixedSource)
        {
            VerifyCS.Test test = new()
            {
                TestCode = originalSource,
                FixedCode = fixedSource,
                MarkupOptions = MarkupOptions.UseFirstDescriptor
            };

            await test.RunAsync();
        }
    }
}