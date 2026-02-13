# OpenTelemetry API PoC Helm Chart
This chart is provided as-is for the OpenTelemetry API PoC project.

## License

```
kubectl exec -it <pod-name> -- env | grep OTEL
```bash

### Verify environment variables

```
kubectl get endpoints
```bash

### Check service endpoints

```
kubectl logs -l app.kubernetes.io/component=secondapi
kubectl logs -l app.kubernetes.io/component=firstapi
```bash

### View pod logs

```
kubectl get pods -l app.kubernetes.io/instance=my-release
```bash

### Check pod status

## Troubleshooting

```
helm install otel-collector open-telemetry/opentelemetry-collector -f otel-collector-values.yaml
helm repo add open-telemetry https://open-telemetry.github.io/opentelemetry-helm-charts
```bash

Install the collector:

```
        exporters: [logging]
        processors: [batch]
        receivers: [otlp]
      logs:
        exporters: [logging]
        processors: [batch]
        receivers: [otlp]
      metrics:
        exporters: [logging]
        processors: [batch]
        receivers: [otlp]
      traces:
    pipelines:
  service:
    # Add your exporters here (e.g., Jaeger, Prometheus, etc.)
      loglevel: debug
    logging:
  exporters:
    batch:
  processors:
          endpoint: 0.0.0.0:4318
        http:
          endpoint: 0.0.0.0:4317
        grpc:
      protocols:
    otlp:
  receivers:
config:
mode: deployment
# otel-collector-values.yaml
```yaml

This chart expects an OpenTelemetry Collector to be available. Here's a simple example:

## OpenTelemetry Collector Setup

```
helm lint ./HelmChart
# Lint the chart

helm template my-release ./HelmChart
# Template to see generated manifests

helm install my-release ./HelmChart --dry-run --debug
# Dry run to validate templates
```bash

## Testing the Chart

```
docker push your-registry.io/secondapi:latest
docker tag secondapi:latest your-registry.io/secondapi:latest

docker push your-registry.io/firstapi:latest
docker tag firstapi:latest your-registry.io/firstapi:latest
# Tag and push to your registry (if using remote registry)

docker build -t secondapi:latest -f SecondApi/Dockerfile .
# Build SecondApi

docker build -t firstapi:latest -f FirstApi/Dockerfile .
# Build FirstApi
```bash

Before deploying, build and push your Docker images:

## Building Docker Images

```
  --set firstApi.autoscaling.targetCPUUtilizationPercentage=70
  --set firstApi.autoscaling.maxReplicas=10 \
  --set firstApi.autoscaling.minReplicas=2 \
  --set firstApi.autoscaling.enabled=true \
helm install my-release ./HelmChart \
```bash

### Deploy with autoscaling

```
  --set firstApi.ingress.hosts[0].paths[0].pathType="Prefix"
  --set firstApi.ingress.hosts[0].paths[0].path="/" \
  --set firstApi.ingress.hosts[0].host="firstapi.example.com" \
  --set firstApi.ingress.enabled=true \
helm install my-release ./HelmChart \
```bash

### Deploy with ingress enabled

```
  --set opentelemetry.serviceNamespace="my-namespace"
  --set opentelemetry.endpoint="http://my-collector:4317" \
helm install my-release ./HelmChart \
```bash

### Deploy with custom OTLP endpoint

```
helm install my-release ./HelmChart --set secondApi.enabled=false
```bash

### Deploy only FirstApi

## Examples

```
helm uninstall my-release
```bash

## Uninstalling the Chart

```
helm upgrade my-release ./HelmChart --set firstApi.replicaCount=3
# Upgrade with inline values

helm upgrade my-release ./HelmChart -f ./HelmChart/values-dev.yaml
# Upgrade with new values
```bash

## Upgrading the Chart

```
      value: "my-value"
    - name: MY_CUSTOM_VAR
  env:
firstApi:
```yaml

Additional environment variables can be added using the `env` array in values.yaml:

- `OTEL_RESOURCE_ATTRIBUTES`: Additional resource attributes including environment and version
- `OTEL_SERVICE_NAMESPACE`: Service namespace for grouping
- `OTEL_SERVICE_NAME`: Service name (first-api or second-api)
- `OTEL_EXPORTER_OTLP_ENDPOINT`: OTLP collector endpoint

The chart automatically configures the following OpenTelemetry environment variables for each API:

## Environment Variables

| `secondApi.autoscaling.enabled` | Enable HPA | `false` |
| `secondApi.ingress.enabled` | Enable ingress | `false` |
| `secondApi.service.port` | Service port | `80` |
| `secondApi.service.type` | Service type | `ClusterIP` |
| `secondApi.image.tag` | Image tag | `latest` |
| `secondApi.image.repository` | Image repository | `secondapi` |
| `secondApi.replicaCount` | Number of replicas | `1` |
| `secondApi.enabled` | Enable SecondApi deployment | `true` |
|-----------|-------------|---------|
| Parameter | Description | Default |

### SecondApi Configuration

| `firstApi.autoscaling.enabled` | Enable HPA | `false` |
| `firstApi.ingress.enabled` | Enable ingress | `false` |
| `firstApi.service.port` | Service port | `80` |
| `firstApi.service.type` | Service type | `ClusterIP` |
| `firstApi.image.tag` | Image tag | `latest` |
| `firstApi.image.repository` | Image repository | `firstapi` |
| `firstApi.replicaCount` | Number of replicas | `1` |
| `firstApi.enabled` | Enable FirstApi deployment | `true` |
|-----------|-------------|---------|
| Parameter | Description | Default |

### FirstApi Configuration

| `opentelemetry.serviceNamespace` | Service namespace for grouping services | `otel-api-poc` |
| `opentelemetry.endpoint` | OTLP endpoint for exporting telemetry | `http://otel-collector:4317` |
|-----------|-------------|---------|
| Parameter | Description | Default |

### OpenTelemetry Configuration

| `global.environment` | Environment name (dev, staging, prod) | `dev` |
|-----------|-------------|---------|
| Parameter | Description | Default |

### Global Configuration

## Configuration

```
helm install otel-api-prod ./HelmChart -f ./HelmChart/values-prod.yaml --namespace prod
# Production environment

helm install otel-api-dev ./HelmChart -f ./HelmChart/values-dev.yaml --namespace dev
# Development environment
```bash

### Installation for Different Environments

```
helm install my-release ./HelmChart --namespace my-namespace --create-namespace
# Install to a specific namespace

helm install my-release ./HelmChart -f ./HelmChart/values-dev.yaml
# Install with custom values file

helm install my-release ./HelmChart
# Install with default values
```bash

### Basic Installation

## Installing the Chart

- An OpenTelemetry Collector endpoint (for OTLP export)
- Helm 3.2.0+
- Kubernetes 1.19+

## Prerequisites

- Optional HorizontalPodAutoscaler
- Optional Ingress
- ServiceAccount
- Service
- Deployment
Both APIs are deployed as separate applications with their own:

- **SecondApi**: A .NET 8 API with OpenTelemetry instrumentation
- **FirstApi**: A .NET 8 API with OpenTelemetry instrumentation
This chart creates separate deployments for:

## Overview

This Helm chart deploys FirstApi and SecondApi with OpenTelemetry (OTLP) support to a Kubernetes cluster.


