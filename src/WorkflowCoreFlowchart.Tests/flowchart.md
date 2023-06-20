```mermaid
flowchart TB
startNode((Start))
0[StepA]
1[StepB]
2[StepA]
3[StepC]
4((End))

startNode --> 0
0 --> 1
1 --> 2
2 --> 3
3 --> 4
```
