# Advanced Algorithms Assignment 1

Prerequisites:

- Node 26
- .NET 10

Instructions

1. Build the backend with:

- `cd Implementation\GaussJordanElim`
- `dotnet build .\GaussJordanElim.slnx`

2. Run WebApi:

- `cd Implementation\GaussJordanElim\SolverApi`
- `dotnet run --launch-profile http`

This will serve on `localhost` on `5070` so make sure it is free.

3. Build the frontend with:

- `cd Implementation\frontend`
- `npm install`
- `npm run build`

4. Run the frontend with:

- `npm run dev`

5. It should look like this:

![Interface image](/Report/Resources/image.png)

---

## Examples

![Example Gaussian](/Report/Resources/example_gauss_2.png)

![Example Magic Square](/Report/Resources/exampleMagicSquare2.png)

![AnotherExample](/Report/Resources/anotherEx.png)

![Last Example](/Report/Resources/lastEx.png)

![LUEx](/Report/Resources/luEx.png)
