# 🤵‍♂️automated proof checker

Ever needed a pair of eyes for your logical proofs? 

Jeorje is an automated proof checker that verifies whether your proof is syntactically valid and semantically correct. 

## 💬 How do I talk to Jeorje?

Is the following statement a valid logical proof? 
```
If it's raining, Jeorje stays home.
If there's free food, Jeorje doesn't stay home. 
There's free food. 

...so it must not be raining! 
```

Let's do some quick formalization:

```
#check ND

is_raining => stay_home(Jeorje),
free_food => !stay_home(Jeorje),
free_food

|- 

!is_raining

1) is_raining => stay_home(Jeorje) premise
2) free_food => !stay_home(Jeorje) premise
3) free_food premise

4) !stay_home(Jeorje) by imp_e on 2,3
5) !is_raining by imp_e on 4,1
```

What does Jeorje think? 

```
Steps:
Successfully scanned input tokens
Found proof format, performing ND
Parsed premises and goal
Rulify ND worked
ND Proof is valid
```

... looks like our natural deduction proof checks out! 

## 🧠 How does it work?

### 🍽️ the *scanner* will turn string input into a list of tokens. 

We'll use a maximal munch algorithm to scan to segment the proof into a list of tokens. The scanner knows the list of acceptable tokens and will yell at us if it doesn't recognize something! 

> `is_raining => stay_home(Jeorje)` will turn into `['is_raining', '=>', 'stay_home', '(', 'Jeorje', ')']`

### 🔌the *transformer* will separate your proof into the header, premise and body sections

Think of this like the preprocessor compiling your code! Some quick analysis on the placement of `,`, `\n` and `|-` lets us divide your proof into sections. We'll also include some quick processing to make some algorithms work later. 

### 🛤️the *parser* will turn each line of the proof into an AST

We'll eventually need to see if every line of the body of the proof can be derived by using rules to transform previous lines. We can't really do that with just a list of tokens! 

This is the job of the *parser*: for each line of the proof, we need to generate an *AST* to help us in checking the validity of rules. 

But before we can talk to the parser, we need to talk to his secretary, the *rulifier*. The job of the rulifier is to figure out which ND rule you're trying to use in the first place. Knowing this, the *parser* can turn the list of tokens into the AST we need with the help of a modified shunting-yard algorithm. 

### 🧑‍⚖️ the *validator* evaluates each line of the proof

With everything in place, the *validator* can check the validity of each line of the proof. He knows every ND rule and will complain if anything's out of place. 

If the validator doesn't find a problem with your proof and the last line matches the the goal we're trying to prove...

### 🥳 we're done! 



*pssst...* If you need a reference for the valid ND rules: 


 `true`, `and_i`, `and_e`, `or_i`, `or_e`, `lem`, `imp_e`, `not_e`, `not_not_i`, `not_not_e`, `iff_i`, `iff_e`, `trans`, `iff_mp`, `exists_i`, `forall_e`, `eq_i`, `eq_e`

---

