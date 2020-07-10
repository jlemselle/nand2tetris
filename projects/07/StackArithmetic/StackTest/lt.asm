// bootstrap
@256
D=A
@SP
M=D

// push constant 17

// D = 17
@17
D=A
// *SP = D
@SP
A=M
M=D
// SP++
@SP
M=M+1

// push constant 18

// D = 17
@18
D=A
// *SP = D
@SP
A=M
M=D
// SP++
@SP
M=M+1

// lt

// SP--
@SP
M=M-1
// D = S[0], M = S[-1]
A=M
D=M
A=A-1
// D = *A + D
D=M-D
// *SP = D
@IS_TRUE
D;JLT
D=0
@IS_FALSE
0;JMP
(IS_TRUE)
D=-1
(IS_FALSE)
@SP
A=M-1
M=D