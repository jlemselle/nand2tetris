// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Mult.asm

// Multiplies R0 and R1 and stores the result in R2.
// (R0, R1, R2 refer to RAM[0], RAM[1], and RAM[2], respectively.)

@R2
M=0

(LOOP)
    // If R0 <= 0 then go to END
    @R0
    D=M
    @END
    D;JLE

    // Reduce R0 by 1
    @R0
    M=M-1

    // R2 += R1
    @R1
    D=M
    @R2
    M=M+D

    // Loop
    @LOOP
    0;JMP

(END) // Infinite loop
    @END
    0;JMP