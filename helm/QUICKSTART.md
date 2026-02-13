# Quick Start Guide - Helm Chart Deployment

Simple guide to deploy FirstApi and SecondApi with local Docker images.

## Prerequisites

- Kubernetes cluster (minikube, kind, or Docker Desktop)
- Helm 3.x installed
- kubectl configured
- Docker

## Step 1: Build Local Docker Images

From the project root directory:

```bash
# Build FirstApi
docker build -t firstapi:latest -t firstapi:1.0 -f FirstApi/Dockerfile .

# Build SecondApi
docker build -t secondapi:latest -t secondapi:1.0 -f SecondApi/Dockerfile .
```

### For Minikube Users

Load images into minikube:

```bash
minikube image load firstapi:latest
minikube image load secondapi:latest
```

### For Kind Users

Load images into kind:

```bash
kind load docker-image firstapi:latest
kind load docker-image secondapi:latest
```

### For Docker Desktop

Images are automatically available in Docker Desktop Kubernetes.

## Step 2: Adjust values.yaml for Local Images

Update the image pull policy to use local images. Create a custom values file or use inline overrides:

```yaml
# values-local.yaml
firstApi:
  image:
    repository: firstapi
    tag: latest
    pullPolicy: IfNotPresent  # or Never for strictly local

secondApi:
  image:
    repository: secondapi
    tag: latest
    pullPolicy: IfNotPresent  # or Never for strictly local

opentelemetry:
  endpoint: "http://otel-collector:4317"  # Adjust if you have a collector
```

## Step 3: Deploy with Helm

### Basic deployment with local images:

```bash
helm install otel-api ./helm/otel-api-poc \
  --set firstApi.image.pullPolicy=IfNotPresent \
  --set secondApi.image.pullPolicy=IfNotPresent
```

### Or using a custom values file:

```bash
helm install otel-api ./helm/otel-api-poc -f values-local.yaml
```

### Deploy to specific namespace:

```bash
kubectl create namespace dev
helm install otel-api ./helm/otel-api-poc \
  --namespace dev \
  --set firstApi.image.pullPolicy=IfNotPresent \
  --set secondApi.image.pullPolicy=IfNotPresent
```

## Step 4: Verify Deployment

```bash
# Check pods are running
kubectl get pods

# Check services
kubectl get services

# View deployment status
helm status otel-api
```

Expected pods:
```
NAME                                              READY   STATUS    RESTARTS   AGE
otel-api-otel-api-poc-firstapi-xxxxxxxxxx-xxxxx   1/1     Running   0          30s
otel-api-otel-api-poc-secondapi-xxxxxxxxxx-xxxxx  1/1     Running   0          30s
```

## Step 5: Access the APIs

### Port forward to access locally:

```bash
# FirstApi on port 8080
kubectl port-forward service/otel-api-otel-api-poc-firstapi 8080:80

# SecondApi on port 8081 (in another terminal)
kubectl port-forward service/otel-api-otel-api-poc-secondapi 8081:80
```

Then access:
- FirstApi: http://localhost:8080
- SecondApi: http://localhost:8081

### View logs:

```bash
# FirstApi logs
kubectl logs -l app.kubernetes.io/component=firstapi -f

# SecondApi logs
kubectl logs -l app.kubernetes.io/component=secondapi -f
```

## Optional: Configure OpenTelemetry Collector

If you need an OTLP collector:

```bash
# Add OpenTelemetry Helm repository
helm repo add open-telemetry https://open-telemetry.github.io/opentelemetry-helm-charts
helm repo update

# Install collector
helm install otel-collector open-telemetry/opentelemetry-collector --set mode=deployment

# Update deployment to use it
helm upgrade otel-api ./helm/otel-api-poc \
  --set opentelemetry.endpoint="http://otel-collector:4317"
```

## Common Commands

### Upgrade deployment:
```bash
helm upgrade otel-api ./helm/otel-api-poc \
  --set firstApi.image.pullPolicy=IfNotPresent \
  --set secondApi.image.pullPolicy=IfNotPresent
```

### Uninstall:
```bash
helm uninstall otel-api
```

### Deploy only one API:
```bash
# Only FirstApi
helm install otel-api ./helm/otel-api-poc \
  --set secondApi.enabled=false \
  --set firstApi.image.pullPolicy=IfNotPresent
```

## Troubleshooting

### If pods are not starting:
```bash
# Check pod status and events
kubectl describe pod <pod-name>

# Check logs
kubectl logs <pod-name>

# Common issue: Image pull error
# Solution: Verify image is loaded and pullPolicy is set correctly
```

### Validate before deploying:
```bash
# Lint the chart
helm lint ./helm/otel-api-poc

# Preview what will be deployed
helm template otel-api ./helm/otel-api-poc \
  --set firstApi.image.pullPolicy=IfNotPresent \
  --set secondApi.image.pullPolicy=IfNotPresent
```

## Summary: Complete Workflow

```bash
# 1. Build images
docker build -t firstapi:latest -f FirstApi/Dockerfile .
docker build -t secondapi:latest -f SecondApi/Dockerfile .

# 2. Load to Kubernetes (if using minikube)
minikube image load firstapi:latest
minikube image load secondapi:latest

# 3. Deploy
helm install otel-api ./helm/otel-api-poc \
  --set firstApi.image.pullPolicy=IfNotPresent \
  --set secondApi.image.pullPolicy=IfNotPresent

# 4. Verify
kubectl get pods

# 5. Access
kubectl port-forward service/otel-api-otel-api-poc-firstapi 8080:80
```

