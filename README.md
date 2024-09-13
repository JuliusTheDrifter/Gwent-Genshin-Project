# Gwent-Genshin-Project
    Gwent-Compilation Project:

Token:
    The Token class and TokenType enum provide a structured way to represent and categorize various elements of source code, which is essential for building interpreters or compilers that process and understand programming languages. The Token class has a lexeme property where the word is saved as a string, a literal property where the value of the word/symbol is saved as an object, an int line and an int column to know where the word is when an error happen.

Lexer:
    The Lexer class is part of a lexical analyzer (or lexer) used in the process of interpreting or compiling code. Its primary job is to scan a source code string and break it down into tokens that can be used by a parser. When a letter is found it keeps loking for other words until another symbol is found. Then, if the letter is a keyword it is saved with preset properties, else, it is saved as an identifier.

SyntaxTree:
    An AST where the Node interface is the backbone of this architecture. It provides a Print method for any class that implements it, allowing objects to print their internal data in a structured way. This feature is helpful for debugging or inspecting objects. 

    CardNode represents an individual card in the game.
    Properties:
        
        Type: The type of card (stored as an Expression).
        
        Name: The name of the card.
        
        Faction: The faction to which the card belongs.
        
        Power: The power rating of the card.
        
        Range: Represents the attack or effect range.
        
        OnActivation: Defines the actions and effects triggered when a card is activated.
        Properties:
            
            OnActivationElements: A list of actions/effects triggered by activation, including the effect itself, any conditions (Selector), and PostActions.
            
            Selector: Defines the conditions for selecting objects in the game, such as which card or entity will be affected by an action.
            
            PostAction: Represents additional actions that should happen after the main action is executed.
            
    EffectNode: represent the effects triggered by certain game events.
    Properties:
        
        Action: Encapsulates a game action, such as applying an effect to a target or context.
    
    ### Expression: The base class for evaluating game logic. Various types of expressions exist. It has an Evaluate method that return the evaluation of the expression as an object.
    Inheritance:
        
        Number: Handles integer values.
        
        String: Handles string values.
        
        Bool: Handles boolean values.
        
        BinaryExpression and UnaryExpression: Handle operations between values, such as addition, subtraction, and logical operations.
        
        Variable: Represents a variable that can hold values, such as Target, Context, or Card.

    Statements such as WhileStatement, ForStatement, and Assignment are used to define control flow and assignment logic in the game. It has an Execute method to executes the statement.
    Inheritance:
        
        WhileStatement: Executes a block of statements while a condition holds true.
        
        ForStatement: Iterates over a list of items, such as cards or game entities.
        
        Assignment: Handles the assignment of values to variables.
        
        Function Class: Represents built-in game functions that can perform actions like modifying a player's hand, deck, or graveyard. Functions take arguments and return values.

Parser:
Key Components:

    1. Tokens and Token Matching: The parser operates on a list of Token objects (Tokens), which represent the individual pieces of input (like keywords, symbols, identifiers, etc).The Match, Check, Peek, Advance, and LookAhead methods are utility functions to navigate through the token list and ensure the tokens match the expected structure.

    2. Parsing Process (Parse Method): The Parse method is the entry point for the parser, which constructs a Program node, representing the entire game script. It loops through the tokens, looking for either a CARD or EFFECT. Based on the token, it calls the appropriate parsing function (ParseCard for cards or ParseEffect for effects). It handles any exceptions that occur during parsing and stores them in the Ex property.

    3. Parsing Cards (ParseCard Method): This method builds a CardNode, which represents an individual card. The card has several expected properties like Type, Name, Faction, Power, Range, and OnActivation. Each of these properties is parsed by looking for a specific token and parsing the associated expression. The method ensures that each card has all required fields (e.g., one Type, Name, Faction, Power, and Range). If a required property is missing, or if there are duplicates, it throws an exception.

    4. Parsing Effects (ParseEffect Method): Similar to cards, the ParseEffect method constructs an EffectNode, which represents an effect in the game. It expects properties like Name, Params, and Action, and processes them accordingly. As with cards, it ensures that required properties are present and not duplicated.

    5. OnActivation and Elements (ParseOnActivation and ParseOAE Methods): OnActivation defines what happens when a card is activated. It is parsed as a list of OnActivationElements, which themselves contain an OAEffect (the main effect of the activation), a Selector (which determines the target of the effect), and any PostAction elements that happen after the activation.

    6. Effect Parsing (ParseOAEffect, ParseSelector, ParsePostAction):
        ParseOAEffect: Handles the parsing of the activation effect, which can include assignments (expressions that modify game variables).

        ParseSelector: Parses a selector, which determines what part of the game (e.g., deck, field, hand) the effect will apply to. It expects properties like Source, Single, and Predicate.

        ParsePostAction: Post-actions are additional actions that happen after the main effect. This method parses those actions.

    7. Parsing Expressions (ParseExpression and Related Methods):
        ParseExpression: The core method that handles parsing all kinds of expressions (arithmetic, boolean, etc). It builds expressions like comparisons, binary operations, and unary operations. The expressions are evaluated in a series of methods like Equality(), Comparison(), Term(), Factor(), Unary(), and Primary().These methods break down the expression into manageable pieces based on operator precedence.

    8. Variable and Function Parsing:
        ParseVariable: This method parses a variable (which can be simple or compound) and supports function calls or accessing properties through dot notation.
        ParseFunction: Parses a function call, which includes the function name and its arguments.

    9. Control Structures (ParseWhileStm, ParseForStm, and Statement Parsing): The parser also supports control structures like while loops and for loops.
        ParseWhileStm: Parses a while loop, which includes a condition and a block of statements.

        ParseForStm: Parses a for loop, which iterates over a set of targets (e.g., a list of cards).
        
        ParseStm: Parses individual statements, such as variable assignments or function calls.

    10. Error Handling
        Consume: This method ensures that the next token is of the expected type, and throws an exception if it's not. It is used frequently to validate the structure of the parsed program.
        The parser throws detailed exceptions when it encounters unexpected tokens or invalid structures, making debugging easier.

Symboltable: 
    SymbolTable class is designed to handle the storage and management of variables and functions in a programming language or scripting environment. It uses a stack-based approach to manage different "scopes" (regions in the code where certain variables are valid), such as inside functions or loops. Here's an explanation of each part of the code.

    Scopes (scopes Stack):
        The class manages a stack of dictionaries where each dictionary represents a scope. A scope holds variables with their names as keys and their types as values (Variable.Type). The stack allows the program to push and pop scopes, which is common in programming languages where scopes are nested (like in functions or blocks).

Context: 
    The Context class acts as a central data store for a card game framework. It holds information about cards, effects, variables, and the game state. This class would be used during gameplay or game logic evaluation, providing access to game components and ensuring the game can manage different cards and effects efficiently.

SemanticalCheck: 
    The SemanticalCheck class performs semantic validation of the Abstract Syntax Tree (AST) nodes within a game script or programming language. It ensures that the nodes are semantically correct, i.e., the types, assignments, function calls, and variable usage conform to the language's rules. This class works in tandem with a Context and a SymbolTable.
    Key Components
    
    Context:
    The Context holds the game's current state, including cards, effects, and variables. It provides information needed to validate the game objects.

    SymbolTable:
    The SymbolTable is used to keep track of variable and function declarations within different scopes. Scopes are pushed and popped as the semantic checker enters and exits different parts of the program.

    Errors:
    The errors list collects any semantic errors encountered during the checks. These errors indicate issues such as type mismatches or undefined variables.
    
    CheckProgramSemantics: Iterates over all EffectNodes and CardNodes in the program, checking their semantics.
    
    CheckCardSemantics: Validates the semantic correctness of a CardNode by:
        Checking the card's type, name, faction, power, and range. 
        Evaluating the card's activation effect (if present). 
        Adding the validated card to the Context.CheckProgramSemantics: Iterates over all EffectNodes and CardNodes in the program, checking their semantics.
    
    CheckEffectSemantics: Pushes a new scope, checks the effect's name, parameters, and actions, then pops the scope. It ensures the effect has valid parameters and actions, and it stores the effect in the Context.
    
    CheckAssignmentSemantics: Ensures that an assignment's left-hand side variable is either previously declared or matches the type of the right-hand side expression. It defines the variable if it's new.
    
    CheckFunctionCall: Validates function calls, checking the number of arguments and their types.
    
    CheckVarCompSemantics: Checks each part of the compound variable and ensures it is being used correctly based on the type of the base variable.

Compile:
    The Compiler script processes text input to generate and display game cards in Unity. It involves lexical scanning, parsing, semantic checking, and then uses the parsed data to instantiate and display card game objects.
    
    Error Handling: Errors encountered during the lexical analysis, parsing, or semantic checking are displayed to the user through UI panels.
    
    Card Management: The SpawnCard method creates card objects and displays them using a prefab and a hand object.

Evaluate:
    The Evaluator class is responsible for evaluating the effects and actions associated with a card when the card is placed, updating the game state as needed, and determining which cards are affected based on selectors.
    
    Card Effects: Evaluates both primary effects and any post-actions, applying them to selected targets.
    
    Selectors and Sources: Handles different sources for card targets (e.g., hand, deck, field) and filters them based on the specified criteria.
    
    Execution: Executes actions and statements that result from evaluating effects and actions and inside of the statements the expressions are evaluated by their inherited method Evaluate().
    