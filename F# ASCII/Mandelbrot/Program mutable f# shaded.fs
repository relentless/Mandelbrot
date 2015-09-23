// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

let xMinScale = -2.5
let xMaxScale = 1.0

let yMinScale = -1.0
let yMaxScale = 1.0

let xMinOuput = 0.0
let xMaxOutput = 79.0

let yMinOutput = 0.0
let yMaxOutput = 24.0

let max_iterations = 1000

let scale inputMin inputMax outputMin outputMax number =
    let onePercent = (number - inputMin) / (inputMax - inputMin)
    let scaled = onePercent * (outputMax - outputMin) + outputMin
    scaled

let scaleX number =
    number |> scale xMinOuput xMaxOutput xMinScale xMaxScale 

let scaleY number =
    number |> scale yMinOutput yMaxOutput yMinScale yMaxScale

let outputs = [| "."; "+"; "o"; "x"; "*"; "#"; "%"; "$"; "&"; "@"; " " |]

[<EntryPoint>]
let main argv = 
    
    for pY in 0.0..yMaxOutput do
        for pX in 0.0..xMaxOutput do
            let x0 = pX |> scaleX
            let y0 = pY |> scaleY

            let mutable x = 0.0
            let mutable y = 0.0

            let mutable iteration = 0

            while x*x + y*y < 2.0*2.0 && iteration < max_iterations do
              
                let xtemp = x*x - y*y + x0
                y <- 2.0*x*y + y0
                x <- xtemp
                iteration <- iteration + 1
              
            let outputIndex = (float iteration) |> scale 0.0 1000.0 0.0 10.0 |> int 
            let outputString = outputs.[outputIndex]
            
            printf "%s" outputString
            
    System.Console.ReadLine() |> ignore
    0 // return an integer exit code
