using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.Common;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Xml;
using System;

public class SemanticalCheck
{
    Context Context;
    SymbolTable symbolTable;
    List<string> errors;
    public SemanticalCheck(Node node, Context context, List<string> errors)
    {
        var program = (node as Program)!;
        symbolTable = new SymbolTable();
        Context = context;
        this.errors = errors;
        CheckProgramSemantics(program);
    }

    public void CheckProgramSemantics(Program program) 
    {
        foreach (var effect in program.EffectNodes)
        {
            CheckEffectSemantics(effect);
        }

        foreach (var card in program.CardNodes)
        {
            CheckCardSemantics(card);
        }
    }

    void CheckCardSemantics(CardNode card)
    {
        symbolTable.PushScope();
        CheckTypeSemantics(card.Type);
        CheckCardNameSemantics(card.Name);
        CheckStringExpression(card.Faction.faction);
        CheckNumericExpression(card.Power.power);
        CheckRangeSemantics(card.Range.range);
        CheckOnActivationSemantics(card.OnActivation.Elements);
        symbolTable.PopScope();
    }

    void CheckEffectSemantics(EffectNode effect)
    {
        symbolTable.PushScope();
        CheckEffectNameSemantics(effect.Name);
        if(effect.Params != null)
        {
            CheckParamsSemantics(effect.Params);
        }
        CheckActionSemantics(effect.Action);
        symbolTable.PopScope();
        Context.effects[Convert.ToString(effect.Name.name.Evaluate())!] = new EffectNode(effect.Name,effect.Params!,effect.Action);
    }

    void CheckTypeSemantics(Type type)
    {
        CheckStringExpression(type.type);
        var trueType = Convert.ToString(type.type.Evaluate());
        if(trueType!="Oro" && trueType!="Plata" && trueType!="Clima" && trueType!="Decoy" && trueType!="Aumento" && trueType!="Despeje")
        {
            errors.Add($"The type : '{type}' is not a valid type.");
        }
    }

    void CheckCardNameSemantics(Name name)
    {
        CheckStringExpression(name.name);
        var trueName = Convert.ToString(name.name.Evaluate())!;
        Context.AddCard(trueName);
    }

    void CheckRangeSemantics(Expression[] expressions)
    {
        foreach(var expression in expressions)
        {
            CheckStringExpression(expression);
            var range = Convert.ToString(expression.Evaluate());
            if(range != "Melee" && range != "Ranged" && range!= "Siege")
            {
                errors.Add($"The range : '{expression}' is not a valid range.");
            }
        }
    }

    void CheckOnActivationSemantics(List<OnActivationElements> onActivationElements)
    {
        foreach(var element in onActivationElements)
        {
            CheckOAElementsSemantics(element);
        }
    }

    void CheckOAElementsSemantics(OnActivationElements oAElements)
    {
        symbolTable.PushScope();
        CheckOAEffect(oAElements.OAEffect);
        CheckSelectorSemantics(oAElements.Selector);
        if(oAElements.postActions != null)
        {
            CheckPostActionSemantics(oAElements.postActions);
        }  
        symbolTable.PopScope();
    }

    void CheckOAEffect(OAEffect oAEffect)
    {
        List<Node> parammeters = Context.GetEffect(oAEffect.Name).Params.Arguments;
        List<Assignment> assignments = oAEffect.Assingments;
        int paramCounter = 0;
        int assignmentCounter = 0;
        foreach(var assignment in assignments)
        {
            assignment.Left.type = InferExpressionType(assignment.Right);
            foreach(var param in parammeters)
            {
                if(assignment.Left.type == (param as Variable)!.type)
                {
                    paramCounter++;
                    assignmentCounter++;
                }
            }
        }
        if(parammeters.Count!=paramCounter || assignments.Count!=assignmentCounter)
        {
            errors.Add(parammeters.Count + " " + assignments.Count);
        }
        foreach(var assignment in oAEffect.Assingments)
        {
            if(assignment.Left.type != GetExpressionType(assignment.Right))
            {
                errors.Add($"The type of the left side of the assignment '{assignment.Left}' is not equal to the right side");
            }
        }
    }

    void CheckSelectorSemantics(Selector selector)
    {
        CheckPredicateSemantics(selector.Predicate);
    }

    void CheckPostActionSemantics(List<PostAction> postActions)
    {
        foreach(var postAction in postActions)
        {
            CheckStringExpression(postAction.Type);
            List<Node> parammeters = Context.GetEffect(Convert.ToString(postAction.Type.Evaluate())!).Params.Arguments;
            List<Assignment> assignments = postAction.Assingments;
            int paramCounter = 0;
            int assignmentCounter = 0;
            foreach(var assignment in assignments)
            {
                foreach(var param in parammeters)
                {
                    if(assignment.Left.type == (param as Variable)!.type)
                    {
                        paramCounter++;
                        assignmentCounter++;
                    }
                }
            }
            if(parammeters.Count!=paramCounter || assignments.Count!=assignmentCounter)
            {
                errors.Add("Params form the PostAction doesn't match the effect params");
            }
            foreach(var assignment in postAction.Assingments)
            {
                if(assignment.Left.type != GetExpressionType(assignment.Right))
                {
                    errors.Add($"The type of the left side of the assignment '{assignment.Left}' is not equal to the right side");
                }
            }
            if(postAction.Selector != null)
            {
                CheckSelectorSemantics(postAction.Selector);
            }
        }
    }

    void CheckPredicateSemantics(Predicate predicate)
    {
        if (predicate.Var.type != Variable.Type.CARD)
        {
            errors.Add($"Predicate variable must be of type CARD, but got {predicate.Var.type}");
        }

        symbolTable.PushScope();
        symbolTable.DefineVariable(predicate.Var.Value, Variable.Type.CARD);
    
        CheckBooleanExpression(predicate.Condition);
    
        symbolTable.PopScope();
    }

    void CheckEffectNameSemantics(Name name)
    {
        CheckStringExpression(name.name);
        string trueName = Convert.ToString(name.name.Evaluate())!;
        Context.AddEffect(trueName);
    }

    void CheckParamsSemantics(Args parameters)
    {
        foreach (var param in parameters.Arguments)
        {
            var trueParam = (param as Variable)!;

            symbolTable.DefineVariable(trueParam.Value, trueParam.type);
        }
    }

    void CheckActionSemantics(Action action)
    {
        symbolTable.PushScope();
        symbolTable.DefineVariable(action.Targets.Value,Variable.Type.TARGETS);
        symbolTable.DefineVariable(action.Context.Value,Variable.Type.CONTEXT);
        CheckStatementsBlockSemantics(action.Block);
        symbolTable.PopScope();
    }

    void CheckStatementsBlockSemantics(StmsBlock block)
    {
        foreach (var stmt in block.statements)
        {
            CheckStatementSemantics(stmt);
        }
    }

    void CheckStatementSemantics(Stmt stmt)
    {
        if(stmt is Assignment assignment)
        {
            CheckAssignmentSemantics(assignment);
        }
        else if(stmt is WhileStatement whileStmt)
        {
            CheckWhileSemantics(whileStmt);
        }
        else if(stmt is ForStatement forStmt)
        {
            CheckForSemantics(forStmt);
        }
        else if(stmt is Function function)
        {
            CheckFunctionCall(function);
        }
        else if(stmt is VariableComp variableComp)
        {
            CheckVarCompSemantics(variableComp);
        }
    }

    void CheckAssignmentSemantics(Assignment assignment)
    {
        if(assignment.Op.Type == TokenType.EQUAL)
        {
            if(symbolTable.ExistsVariable(assignment.Left.Value))
            {
                Variable.Type type = symbolTable.LookupVariable(assignment.Left.Value);
                if(type != InferExpressionType(assignment.Right))
                {
                    errors.Add($"The type of the left side of the assignment '{assignment.Left}' is not equal to the right side");
                }
            }
            else
            {
                symbolTable.DefineVariable(assignment.Left.Value,InferExpressionType(assignment.Right));
            }
        }
        else
        {
            if(assignment.Left is Variable)
            {
                CheckVariableUsage(assignment.Left);
            }
            else
            {
                CheckVarCompSemantics((assignment.Left as VariableComp)!);
            }
            if(assignment.Left.type != InferExpressionType(assignment.Right))
            {
                errors.Add($"The type of the left side of the assignment '{assignment.Left}' is not equal to the right side");
            }
            if(assignment.Left.type != Variable.Type.INT && assignment.Left.type != Variable.Type.STRING)
            {
                errors.Add($"The type of the left side of the assignment '{assignment.Left}' is not equal to the right side");
            }
        }
    }

    void CheckForSemantics(ForStatement forStmt)
    {
        symbolTable.PushScope();
        symbolTable.DefineVariable(forStmt.Target.Value,Variable.Type.CARD);
        CheckVariableUsage(forStmt.Targets);
        CheckStatementsBlockSemantics(forStmt.Body);
        symbolTable.PopScope();
    }

    void CheckWhileSemantics(WhileStatement whileStmt)
    {
        symbolTable.PushScope();
        var variablesInCondition = FindVariablesInExpression(whileStmt.Condition);
        foreach (var variable in variablesInCondition)
        {
            try
            {
                symbolTable.LookupVariable(variable.Value);
            }
            catch (Exception)
            {
                errors.Add($"Variable '{variable.Value}' in while condition is not declared.");
            }
        }
        CheckBooleanExpression(whileStmt.Condition);
        CheckStatementsBlockSemantics(whileStmt.Body);
        symbolTable.PopScope();
    }

    void CheckVariableUsage( Variable variable)
    {
        try
        {
            var varType = symbolTable.LookupVariable(variable.Value);
            Console.WriteLine($"Variable '{variable.Value}' is of type {varType}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    void CheckFunctionCall( Function function)
    {
        try
        {
            var returnType = symbolTable.LookupFunction(function.FunctionName);
            Console.WriteLine($"Function '{function.FunctionName}' returns type {returnType}");
            // Check argument types and count
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    Variable.Type GetExpressionType(Expression expression)
    {
        if(expression is String || expression is BinaryStringExpression)
        {
            CheckStringExpression(expression);
            return Variable.Type.STRING;
        }
        else if(expression is Number || expression is UnaryIntergerExpression || expression is BinaryIntergerExpression)
        {
            CheckNumericExpression(expression);
            return Variable.Type.INT;
        }
        else if(expression is Bool || expression is UnaryBooleanExpression || expression is BinaryBooleanExpression)
        {
            CheckBooleanExpression(expression);
            return Variable.Type.BOOL;
        }
        else if(expression is ExpressionGroup)
        {
            var expressionGroup = (expression as ExpressionGroup)!;
            return GetExpressionType(expressionGroup.Exp);
        }
        else
        {
            if(expression is VariableComp)
            {
                VariableComp variableComp = (expression as VariableComp)!;
                CheckVarCompSemantics(variableComp);
                return variableComp.type;
            }
            else
            {
                Variable variable = (expression as Variable)!;
                return variable.type;
            }
        }
    }

    void CheckVarCompSemantics(VariableComp variableComp)
    {
        Variable.Type currentType = symbolTable.LookupVariable(variableComp.Value);
    
        foreach (var arg in variableComp.args.Arguments)
        {
            if (arg is Function function)
            {
                CheckFunctionCall(function);
                currentType = function.Type;
            }
            else if (arg is Type || arg is Name || arg is Faction || arg is PowerAsField || arg is Range || arg is Pointer)
            {
                
            }
        }
    
        variableComp.type = currentType;
    }

    void CheckStringExpression(Expression expression)
    {
        try
        {
            if(expression is String)
            {
                return;
            }
            else if(expression is BinaryStringExpression)
            {
                var binaryStringExpression = (expression as BinaryStringExpression)!;
                CheckStringExpression(binaryStringExpression.Left);
                CheckStringExpression(binaryStringExpression.Right);
            }
            else if(expression is ExpressionGroup)
            {
                var expressionGroup = (expression as ExpressionGroup)!;
                CheckStringExpression(expressionGroup.Exp);
            }
            else if(expression is Variable)
            {
                if(expression is VariableComp)
                {
                    VariableComp variableComp = (expression as VariableComp)!;
                    CheckVarCompSemantics(variableComp);
                    if(variableComp.type != Variable.Type.STRING)
                    {
                        errors.Add($"A string was expected but instead got {variableComp.type}");
                    }
                }
                else
                {
                    Variable variable = (expression as Variable)!;
                    symbolTable.LookupVariable(variable.Value);
                    if(variable.type != Variable.Type.STRING)
                    {
                        errors.Add($"A string was expected but instead got {variable.type}");
                    }
                }
            }
            else
            {

            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    void CheckNumericExpression(Expression expression)
    {
        try
        {
            if(expression is Number)
            {
                return;
            }
            else if(expression is UnaryIntergerExpression)
            {
                var unaryIntergerExpression = (expression as UnaryIntergerExpression)!;
                CheckNumericExpression(unaryIntergerExpression.Right);
            }
            else if(expression is BinaryIntergerExpression)
            {
                var binaryIntergerExpression = (expression as BinaryIntergerExpression)!;
                CheckNumericExpression(binaryIntergerExpression.Left);
                CheckNumericExpression(binaryIntergerExpression.Right);
            }
            else if(expression is ExpressionGroup)
            {
                var expressionGroup = (expression as ExpressionGroup)!;
                CheckNumericExpression(expressionGroup.Exp);
            }
            else if(expression is Variable)
            {
                if(expression is VariableComp)
                {
                    VariableComp variableComp = (expression as VariableComp)!;
                    CheckVarCompSemantics(variableComp);
                    if(variableComp.type != Variable.Type.INT)
                    {
                        errors.Add($"A number was expected but instead got {variableComp.type}");
                    }
                }
                else
                {
                    Variable variable = (expression as Variable)!;
                    symbolTable.LookupVariable(variable.Value);
                    if(variable.type != Variable.Type.INT)
                    {
                        errors.Add($"A number was expected but instead got {variable.type}");
                    }
                }
            }
            else
            {

            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    void CheckBooleanExpression(Expression expression)
    {
        try
        {
            if(expression is Bool)
            {
                return;
            }
            else if(expression is UnaryBooleanExpression)
            {
                var unaryBooleanExpression = (expression as UnaryBooleanExpression)!;
                CheckBooleanExpression(unaryBooleanExpression.Right);
            }
            else if(expression is BinaryBooleanExpression)
            {
                var binaryBooleanExpression = (expression as BinaryBooleanExpression)!;

                if(binaryBooleanExpression.Operators.Type == TokenType.GREATER||binaryBooleanExpression.Operators.Type == TokenType.EQUAL_GREATER||
                binaryBooleanExpression.Operators.Type == TokenType.LESS||binaryBooleanExpression.Operators.Type == TokenType.LESS_EQUAL)
                {
                    CheckNumericExpression(binaryBooleanExpression.Left);
                    CheckNumericExpression(binaryBooleanExpression.Right);
                }
                else if(binaryBooleanExpression.Operators.Type == TokenType.BANG_EQUAL||binaryBooleanExpression.Operators.Type == TokenType.EQUAL_EQUAL)
                {
                    var leftType = InferExpressionType(binaryBooleanExpression.Left);
                    var rightType = InferExpressionType(binaryBooleanExpression.Right);
                    if(!AreCompatibleTypes(leftType, rightType))
                    {
                        errors.Add($"Incompatible types in comparison: {leftType} and {rightType}");
                    }
                    if(leftType == Variable.Type.INT)
                    {
                        CheckNumericExpression(binaryBooleanExpression.Left);
                        CheckNumericExpression(binaryBooleanExpression.Right);
                    }
                    else if(leftType == Variable.Type.STRING)
                    {
                        CheckStringExpression(binaryBooleanExpression.Left);
                        CheckStringExpression(binaryBooleanExpression.Right);
                    }
                    else if(leftType == Variable.Type.BOOL)
                    {
                        CheckBooleanExpression(binaryBooleanExpression.Left);
                        CheckBooleanExpression(binaryBooleanExpression.Right);
                    }
                }
            }
            else if(expression is ExpressionGroup)
            {
                var expressionGroup = (expression as ExpressionGroup)!;
                CheckBooleanExpression(expressionGroup.Exp);
            }
            else if (expression is Variable variable)
            {
                var varType = symbolTable.LookupVariable(variable.Value);
                if (varType != Variable.Type.BOOL)
                {
                    errors.Add($"Variable '{variable.Value}' is not of type BOOL");
                }
            }
            else if (expression is VariableComp variableComp)
            {
                CheckVarCompSemantics(variableComp);
                if (variableComp.type != Variable.Type.BOOL)
                {
                    errors.Add($"Expression does not evaluate to a BOOL type");
                }
            }
            else
            {
                errors.Add($"Invalid expression type for boolean context: {expression.GetType().Name}");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    List<Variable> FindVariablesInExpression(Expression expression)
    {
        var variables = new List<Variable>();
        if (expression is Variable variable)
        {
            variables.Add(variable);
        }
        else if (expression is BinaryExpression binaryExpression)
        {
            variables.AddRange(FindVariablesInExpression(binaryExpression.Left));
            variables.AddRange(FindVariablesInExpression(binaryExpression.Right));
        }
        else if (expression is UnaryExpression unaryExpression)
        {
            variables.AddRange(FindVariablesInExpression(unaryExpression.Right));
        }
        return variables;
    }

    Variable.Type InferExpressionType(Expression expression)
    {
        if (expression is Number)
        {
            return Variable.Type.INT;
        }
        else if (expression is String)
        {
            return Variable.Type.STRING;
        }
        else if(expression is Bool)
        {
            return Variable.Type.BOOL;
        }
        else if(expression is VariableComp variableComp)
        {
            return variableComp.type;
        }
        else if(expression is Variable variable)
        {
            return symbolTable.LookupVariable(variable.Value);
        }
        else
        {
            return InferExpressionType((expression as ExpressionGroup).Exp);
        }
    }

    bool AreCompatibleTypes(Variable.Type leftType, Variable.Type rightType)
    {
        return leftType == rightType;
    }
}
