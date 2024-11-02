# Deterministic Finite Automata

### Example

- Regex: `a(a|b)b`

```mermaid
flowchart LR
  start(((Start))):::startNode --> q0(("q0")):::normal
  q0 --> |a| q1(("q1")):::normal
  q1 --> |a| q2(("q2")):::normal
  q1 --> |b| q2
  q2 --> |b| q3((("q3"))):::finalState

  classDef startNode fill:none,stroke:none;
  classDef normal stroke:#000,stroke-width:2px;
  classDef finalState stroke:#000,stroke-width:3px;

  linkStyle 0 stroke-width:2px;
  linkStyle 1,2,3 stroke:#333,stroke-width:2px,fill:none;
```

### Reserved_Keywords

- Regex: `int | float | string | read | write | repeat | until | if | elseif | else | then | return | endl | end | main`

```mermaid
flowchart LR
  start(((Start))):::startNode --> q0(("q0")):::normal
  q0 --> |int| q1((("q1"))):::finalState
  q0 --> |float| q2((("q2"))):::finalState
  q0 --> |string| q3((("q3"))):::finalState
  q0 --> |read| q4((("q4"))):::finalState
  q0 --> |write| q5((("q5"))):::finalState
  q0 --> |repeat| q6((("q6"))):::finalState
  q0 --> |until| q7((("q7"))):::finalState
  q0 --> |if| q8((("q8"))):::finalState
  q0 --> |elseif| q9((("q9"))):::finalState
  q0 --> |else| q10((("q10"))):::finalState
  q0 --> |then| q11((("q11"))):::finalState
  q0 --> |return| q12((("q12"))):::finalState
  q0 --> |endl| q13((("q13"))):::finalState
  q0 --> |end| q14((("q14"))):::finalState
  q0 --> |main| q15((("q15"))):::finalState


  classDef startNode fill:none,stroke:none;
  classDef normal stroke:#000,stroke-width:2px;
  classDef finalState stroke:#000,stroke-width:3px;

  linkStyle 0 stroke-width:2px;
  linkStyle 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15 stroke:#333,stroke-width:2px,fill:none;

```

### Arithmetic_Operators

- Regex: `\+|-|\*|\/`

<!-- I am using these HTML character codes (e.g. #43;) since directly using '+' or '-' or '*' gives syntax errors with the mermaid syntax -->

```mermaid
flowchart LR
  start(((Start))):::startNode --> q0(("q0")):::normal
  q0 --> |#43;| q1((("q1"))):::finalState
  q0 --> |#45;| q2((("q2"))):::finalState
  q0 --> |#42;| q3((("q3"))):::finalState
  q0 --> |#47;| q4((("q4"))):::finalState

  classDef startNode fill:none,stroke:none;
  classDef normal stroke:#000,stroke-width:2px;
  classDef finalState stroke:#000,stroke-width:3px;

  linkStyle 0 stroke-width:2px;
  linkStyle 1,2,3,4 stroke:#333,stroke-width:2px,fill:none;
```

### Assignment_Operator

- Regex: `:=`

```mermaid
flowchart LR
  start(((Start))):::startNode --> q0(("q0")):::normal
  q0 --> |:| q1(("q1")):::normal
  q1 --> |=| q2((("q2"))):::finalState

  classDef startNode fill:none,stroke:none;
  classDef normal stroke:#000,stroke-width:2px;
  classDef finalState stroke:#000,stroke-width:3px;

  linkStyle 0 stroke-width:2px;
  linkStyle 1,2 stroke:#333,stroke-width:2px,fill:none;
```

### Delimiters
```mermaid
flowchart LR
  start(((Start))):::startNode --> q0(("q0")):::normal
  q0 --> |"{"| q1((("q1"))):::finalState
  q0 --> |"}"| q2((("q2"))):::finalState
  q0 --> |";"| q3((("q3"))):::finalState
  q0 --> |"."| q4((("q4"))):::finalState
  q0 --> |","| q5((("q5"))):::finalState

  classDef startNode fill:none,stroke:none;
  classDef normal stroke:#000,stroke-width:2px;
  classDef finalState stroke:#000,stroke-width:3px;

  linkStyle 0 stroke-width:2px;
  linkStyle 1 stroke:#333,stroke-width:2px,fill:none;

```