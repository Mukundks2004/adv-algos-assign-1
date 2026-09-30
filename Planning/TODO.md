# TODOs

In the code there are lots of mentions of "clone" when referring to 0 and 1 to avoid mutating the static variables. E.g.

```cs
result[row, size * size] = T.Zero.Clone() - T.One.Clone();
```

Fix this by putting some kind of harness around it that auto clones it.
