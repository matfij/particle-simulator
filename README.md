# Particle Simulator

C# .NET MAUI physics engine, rendering with SkiaSharp Canvas.

![CI](https://github.com/matfij/particle-simulator/actions/workflows/build.yml/badge.svg)

---

# AWS CDK Backend

C# AWS CDK serverless application for sharing simulation data. Deployment is managed by Cloud Formation, logic is executed by Lambda Functions, simulation data is stored in S3 Bucket, simulation data reference is stored in DynamoDB.

## Requirements

- VS 2022+
- AWS CDK CLI

## Setup

- bootstrap AWS CDK CLI: `cdk bootstrap`
- package lambda functions: `dotnet lambda package`
- synthesize stack: `cdk synth`
- deploy stack: `cdk deploy`

## Deleting resources

- delete stack: `cdk destroy`
