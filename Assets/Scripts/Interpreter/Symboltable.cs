using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Xml;
using System;

public class SymbolTable
{
    Stack<Dictionary<string, Variable.Type>> scopes = new Stack<Dictionary<string, Variable.Type>>();
    Dictionary<string, Variable.Type> functions = new Dictionary<string, Variable.Type>();

    public SymbolTable()
    {
        PushScope();
    }

    public void PushScope()
    {
        scopes.Push(new Dictionary<string, Variable.Type>());
    }

    public void PopScope()
    {
        if (scopes.Count > 0)
        {
            scopes.Pop();
        }
        else
        {
            throw new Exception("No more scopes to pop.");
        }
    }

     public void DefineVariable(string name, Variable.Type type)
    {
        foreach (var scope in scopes)
        {
            if (scope.ContainsKey(name))
            {
                throw new Exception($"Variable '{name}' ya está definida en este scope.");
            }
        }
        var currentScope = scopes.Peek();
        currentScope[name] = type;
    }

    public void DefineFunction(string name, Variable.Type returnType)
    {
        if (functions.ContainsKey(name))
        {
            throw new Exception($"Function '{name}' ya está definida.");
        }
        functions[name] = returnType;
    }

    public Variable.Type LookupVariable(string name)
    {
        foreach (var scope in scopes)
        {
            if (scope.ContainsKey(name))
            {
                return scope[name];
            }
        }
        throw new Exception($"Variable '{name}' no ha sido declarada.");
    }

    public Variable.Type LookupFunction(string name)
    {
        if (functions.ContainsKey(name))
        {
            return functions[name];
        }
        throw new Exception($"Function '{name}' no ha sido declarada.");
    }

    public bool ExistsVariable(string name)
    {
        foreach (var scope in scopes)
        {
            if (scope.ContainsKey(name))
            {
                return true;
            }
        }
        return false;
    }

    /*public void AddVariable(string name, Variable.Type type)
    {
        var currentScope = scopes.Peek();
        if (currentScope.ContainsKey(name))
        {
            throw new Exception($"Variable '{name}' ya está definida en este scope.");
        }
        currentScope[name] = type;
    }

    public void AddFunction()
    {

    }*/

}