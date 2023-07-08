```mermaid
flowchart TB
startNode((Start))
0[StepA]
1[StepB]
2{"(data.Param1 > 96)"}
3[StepA]

startNode --> 0
0 --> 1
1 --> 2
2 -->|false| 1
2 -->|true| 3
3 --> 0
```
