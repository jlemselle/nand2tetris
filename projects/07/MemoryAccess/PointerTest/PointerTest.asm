// PUSH CONSTANT 3030

@3030
D=A
@SP
A=M
M=D
@SP
M=M+1


// POP POINTER 0

@SP
AM=M-1
D=M
@3
M=D


// PUSH CONSTANT 3040

@3040
D=A
@SP
A=M
M=D
@SP
M=M+1


// POP POINTER 1

@SP
AM=M-1
D=M
@4
M=D


// PUSH CONSTANT 32

@32
D=A
@SP
A=M
M=D
@SP
M=M+1


// POP THIS 2

@THIS
D=M
@2
D=D+A
@R13
M=D
@SP
AM=M-1
D=M
@R13
A=M
M=D


// PUSH CONSTANT 46

@46
D=A
@SP
A=M
M=D
@SP
M=M+1


// POP THAT 6

@THAT
D=M
@6
D=D+A
@R13
M=D
@SP
AM=M-1
D=M
@R13
A=M
M=D


// PUSH POINTER 0

@3
D=M
@SP
A=M
M=D
@SP
M=M+1


// PUSH POINTER 1

@4
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


// PUSH THIS 2

@THIS
D=M
@2
A=D+A
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


// PUSH THAT 6

@THAT
D=M
@6
A=D+A
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

