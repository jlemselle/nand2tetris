// PUSH

@17
D=A
@SP
A=M
M=D
@SP
M=M+1


// PUSH

@17
D=A
@SP
A=M
M=D
@SP
M=M+1


// eq

@SP
M=M-1
A=M
D=M
A=A-1
D=M-D
@IS_ZERO.0
D;JEQ
D=-1
(IS_ZERO.0)
@SP
A=M-1
M=!D


// PUSH

@17
D=A
@SP
A=M
M=D
@SP
M=M+1


// PUSH

@16
D=A
@SP
A=M
M=D
@SP
M=M+1


// eq

@SP
M=M-1
A=M
D=M
A=A-1
D=M-D
@IS_ZERO.1
D;JEQ
D=-1
(IS_ZERO.1)
@SP
A=M-1
M=!D


// PUSH

@16
D=A
@SP
A=M
M=D
@SP
M=M+1


// PUSH

@17
D=A
@SP
A=M
M=D
@SP
M=M+1


// eq

@SP
M=M-1
A=M
D=M
A=A-1
D=M-D
@IS_ZERO.2
D;JEQ
D=-1
(IS_ZERO.2)
@SP
A=M-1
M=!D


// PUSH

@892
D=A
@SP
A=M
M=D
@SP
M=M+1


// PUSH

@891
D=A
@SP
A=M
M=D
@SP
M=M+1


// lt

@SP
M=M-1
A=M
D=M
A=A-1
D=M-D
@IS_TRUE.3
D;JLT
D=0
@IS_FALSE.3
0;JMP
(IS_TRUE.3)
D=-1
(IS_FALSE.3)
@SP
A=M-1
M=D


// PUSH

@891
D=A
@SP
A=M
M=D
@SP
M=M+1


// PUSH

@892
D=A
@SP
A=M
M=D
@SP
M=M+1


// lt

@SP
M=M-1
A=M
D=M
A=A-1
D=M-D
@IS_TRUE.4
D;JLT
D=0
@IS_FALSE.4
0;JMP
(IS_TRUE.4)
D=-1
(IS_FALSE.4)
@SP
A=M-1
M=D


// PUSH

@891
D=A
@SP
A=M
M=D
@SP
M=M+1


// PUSH

@891
D=A
@SP
A=M
M=D
@SP
M=M+1


// lt

@SP
M=M-1
A=M
D=M
A=A-1
D=M-D
@IS_TRUE.5
D;JLT
D=0
@IS_FALSE.5
0;JMP
(IS_TRUE.5)
D=-1
(IS_FALSE.5)
@SP
A=M-1
M=D


// PUSH

@32767
D=A
@SP
A=M
M=D
@SP
M=M+1


// PUSH

@32766
D=A
@SP
A=M
M=D
@SP
M=M+1


// gt

@SP
M=M-1
A=M
D=M
A=A-1
D=M-D
@IS_TRUE.6
D;JGT
D=0
@IS_FALSE.6
0;JMP
(IS_TRUE.6)
D=-1
(IS_FALSE.6)
@SP
A=M-1
M=D


// PUSH

@32766
D=A
@SP
A=M
M=D
@SP
M=M+1


// PUSH

@32767
D=A
@SP
A=M
M=D
@SP
M=M+1


// gt

@SP
M=M-1
A=M
D=M
A=A-1
D=M-D
@IS_TRUE.7
D;JGT
D=0
@IS_FALSE.7
0;JMP
(IS_TRUE.7)
D=-1
(IS_FALSE.7)
@SP
A=M-1
M=D


// PUSH

@32766
D=A
@SP
A=M
M=D
@SP
M=M+1


// PUSH

@32766
D=A
@SP
A=M
M=D
@SP
M=M+1


// gt

@SP
M=M-1
A=M
D=M
A=A-1
D=M-D
@IS_TRUE.8
D;JGT
D=0
@IS_FALSE.8
0;JMP
(IS_TRUE.8)
D=-1
(IS_FALSE.8)
@SP
A=M-1
M=D


// PUSH

@57
D=A
@SP
A=M
M=D
@SP
M=M+1


// PUSH

@31
D=A
@SP
A=M
M=D
@SP
M=M+1


// PUSH

@53
D=A
@SP
A=M
M=D
@SP
M=M+1


// add

@SP
M=M-1
A=M
D=M
A=A-1
D=M+D
M=D


// PUSH

@112
D=A
@SP
A=M
M=D
@SP
M=M+1


// sub

@SP
M=M-1
A=M
D=M
A=A-1
D=M-D
M=D


// neg

@SP
A=M-1
D=M
M=-D


// and

@SP
M=M-1
A=M
D=M
A=A-1
D=M&D
M=D


// PUSH

@82
D=A
@SP
A=M
M=D
@SP
M=M+1


// or

@SP
M=M-1
A=M
D=M
A=A-1
D=M|D
M=D


// not

@SP
A=M-1
D=M
M=!D


