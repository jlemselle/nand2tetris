// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Fill.asm

// Runs an infinite loop that listens to the keyboard input.
// When a key is pressed (any key), the program blackens the screen,
// i.e. writes "black" in every pixel;
// the screen should remain fully black as long as the key is pressed. 
// When no key is pressed, the program clears the screen, i.e. writes
// "white" in every pixel;
// the screen should remain fully clear as long as no key is pressed.


(WAITWHITE)
    @24576
    D=M
    @PAINTBLACK
    D;JNE

    // Loop
    @WAITWHITE
    0;JMP


(WAITBLACK)
    @24576
    D=M
    @PAINTWHITE
    D;JEQ

    // Loop
    @WAITBLACK
    0;JMP

(PAINTBLACK)
    @8192
    D=A

(BLACK)
    // Write 0xFFFF to 16384 + D
    @16384
    A=A+D
    M=-1

    // Jump to WAITBLACK if D <= 0
    @WAITBLACK
    D=D-1;JLT

    // Loop
    @BLACK
    0;JMP

(PAINTWHITE)
    @8192
    D=A

(WHITE)
    // Write 0xFFFF to 16384 + D
    @16384
    A=A+D
    M=0

    // Jump to WAITWHITE if D <= 0
    @WAITWHITE
    D=D-1;JLT

    // Loop
    @WHITE
    0;JMP

(END) // Infinite loop
    @END
    0;JMP