// bootstrap
@256
D=A
@SP
M=D

// push constant x

// D = x
@18
D=A
// *SP = D
@SP
A=M
M=D
// SP++
@SP
M=M+1

// push constant x

// D = x
@18
D=A
// *SP = D
@SP
A=M
M=D
// SP++
@SP
M=M+1

// eq

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
D=M-D
// *SP = D
@IS_ZERO
D;JEQ
D=-1
(IS_ZERO)
@SP
A=M-1
M=!D

// not

@SP
A=M-1
D=M
M=!D