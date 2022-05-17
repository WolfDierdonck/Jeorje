# Jeorje
Alternate version of an automated proof checker named after Curious George, the monkey.

Jeorje is an automated proof checker that verifies correctness of proofs in natural deduction (and semantic tableaux + transformational proof in the future).
Jeorje will also handle program correctness and z-specification rule/type checking. 

George uses maximal munch to scan input tokens in predicate logic, and then uses a modified version of the railroad-shunt algorithm to generate a parse tree. Then, rules are checked to ensure correct usage.
