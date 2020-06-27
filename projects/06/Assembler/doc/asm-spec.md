// comment  
(LABEL)

# Tokens

comment
symbol ( ) @ = ; - ! + & |
identifier [a-zA-Z_\.\$:][a-zA-Z0-9_\.\$:]*
number
D
A
M
JGT
JEQ
JGE
JLT
JNE
JLE
JMP
newline
T
# Symbol

A user-defined symbol can be any sequence of letters, digits, underscores (_), dot (.), dollar sign ($), and colon (:) that does not begin with a digit.

# A-instruction

0 v v v - v v v v - v v v v - v v v v

@_value_

where _value_ is either a decimal number between 0 and 32,767 or a symbol referring to such number.

# C-instruction

1 1 1 a - c1 c2 c3 c4 - c5 c6 d1 d2 - d3 j1 j2 j3

[_dest_=]_comp_[;_jump_]

Either the _dest_ or _jump_ fields may be empty.

## Comp

| comp | m (0 = A, 1 = M) | zd (D = 0) | nd (D = !D) | zam (A/M = 0) | nam (A/M = !A/M) | add (0 = &, 1 = +) | nout (out = !out) |
|-|-|-|-|-|-|-|-|
| 0 | X | 1 | 0 | 1 | 0 | 1 | 0 |
| 1 | X | 1 | 1 | 1 | 1 | 1 | 1 |
| -1 | X | 1 | 1 | 1 | 0 | 1 | 0 |
| D | X | 0 | 0 | 1 | 1 | 0 | 0 |
| A | 0 | 1 | 1 | 0 | 0 | 0 | 0 |
| M | 1 | 1 | 1 | 0 | 0 | 0 | 0 |
| !D | X | 0 | 0 | 1 | 1 | 0 | 1 |
| !A | 0 | 1 | 1 | 0 | 0 | 0 | 1 |
| !M | 1 | 1 | 1 | 0 | 0 | 0 | 1 |
| -D | X | 0 | 0 | 1 | 1 | 1 | 1 |
| -A | 0 | 1 | 1 | 0 | 0 | 1 | 1 |
| -M | 1 | 1 | 1 | 0 | 0 | 1 | 1 |
| D+1 | X | 0 | 1 | 1 | 1 | 1 | 1 |
| A+1 | 0 | 1 | 1 | 0 | 1 | 1 | 1 |
| M+1 | 1 | 1 | 1 | 0 | 1 | 1 | 1 |
| D-1 | X | 0 | 0 | 1 | 1 | 1 | 0 |
| A-1 | 0 | 1 | 1 | 0 | 0 | 1 | 0 |
| M-1 | 1 | 1 | 1 | 0 | 0 | 1 | 0 |
| D+A | 0 | 0 | 0 | 0 | 0 | 1 | 0 |
| D+M | 1 | 0 | 0 | 0 | 0 | 1 | 0 |
| D-A | 0 | 0 | 1 | 0 | 0 | 1 | 1 |
| D-M | 1 | 0 | 1 | 0 | 0 | 1 | 1 |
| A-D | 0 | 0 | 0 | 0 | 1 | 1 | 1 |
| M-D | 1 | 0 | 0 | 0 | 1 | 1 | 1 |
| D&A | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| D&M | 1 | 0 | 0 | 0 | 0 | 0 | 0 |
| D\|A | 0 | 0 | 1 | 0 | 1 | 0 | 1 |
| D\|M | 1 | 0 | 1 | 0 | 1 | 0 | 1 |

## Dest

| dest | d1 (A = out) | d2 (D = out) | d3 (M = out) |
|-|-|-|-|
| null | 0 | 0 | 0 |
| M | 0 | 0 | 1 |
| D | 0 | 1 | 0 |
| MD | 0 | 1 | 1 |
| A | 1 | 0 | 0 |
| AM | 1 | 0 | 1 |
| AD | 1 | 1 | 0 |
| AMD | 1 | 1 | 1 |

## Jump

| jump | j1 (out < 0) | j2 (out = 0) | j3 (out > 0) |
|-|-|-|-|
| null (false) | 0 | 0 | 0 |
| JGT (out > 0) | 0 | 0 | 1 |
| JEQ (out = 0) | 0 | 1 | 0 |
| JGE (out >= 0) | 0 | 1 | 1 |
| JLT (out < 0) | 1 | 0 | 0 |
| JNE (ou != 0) | 1 | 0 | 1 |
| JLE (out <= 0) | 1 | 1 | 0 |
| JMP (true) | 1 | 1 | 1 |