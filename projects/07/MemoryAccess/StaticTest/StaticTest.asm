// PUSH CONSTANT 111

@111
D=A
@SP
A=M
M=D
@SP
M=M+1


// PUSH CONSTANT 333

@333
D=A
@SP
A=M
M=D
@SP
M=M+1


// PUSH CONSTANT 888

@888
D=A
@SP
A=M
M=D
@SP
M=M+1


// POP STATIC 8

@SP
AM=M-1
D=M
@StaticTest.8
M=D


// POP STATIC 3

@SP
AM=M-1
D=M
@StaticTest.3
M=D


// POP STATIC 1

@SP
AM=M-1
D=M
@StaticTest.1
M=D


// PUSH STATIC 3

@StaticTest.3
D=M
@SP
A=M
M=D
@SP
M=M+1


// PUSH STATIC 1

@StaticTest.1
D=M
@SP
A=M
M=D
@SP
M=M+1


// SUB

@SP
M=M-1
A=M
D=M
A=A-1
D=M-D
M=D


// PUSH STATIC 8

@StaticTest.8
D=M
@SP
A=M
M=D
@SP
M=M+1


// ADD

@SP
M=M-1
A=M
D=M
A=A-1
D=M+D
M=D


