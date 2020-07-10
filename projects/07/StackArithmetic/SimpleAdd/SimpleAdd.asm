// push constant 7

// D = 7
@7
D=A
// *SP = D
@SP
A=M
M=D
// SP++
@SP
M=M+1

// push constant 8

// D = 8
@8
D=A
// *SP = D
@SP
A=M
M=D
// SP++
@SP
M=M+1

// add

// SP--
@SP
M=M-1
// A = *SP
A=M
// D = *A
D=M
// A = *SP - 1
A=A-1
// D = *A + D
D=M+D
// *SP = D
M=D