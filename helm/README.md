# Helm Chart - OpenTelemetry API PoC

## Overview

This Helm chart deploys **FirstApi** and **SecondApi** as separate applications in Kubernetes with full OpenTelemetry (OTLP) support.

## ✅ What's Included

### Chart Structure
```
helm/
├── otel-api-poc/              # Main Helm chart
│   ├── Chart.yaml             # Chart metadata
│   ├── values.yaml            # Default configuration values
│   ├── values-dev.yaml        # Development environment configuration
│   ├── values-prod.yaml       # Production environment configuration
│   ├── README.md              # Comprehensive chart documentation
│   └── templates/             # Kubernetes manifest templates
│       ├── firstapi-*         # FirstApi resources (5 files)
│       ├── secondapi-*        # SecondApi resources (5 files)
│       ├── _helpers.tpl       # Template helpers
│       └── NOTES.txt          # Post-install instructions
└── QUICKSTART.md              # Quick deployment guide
```

## 🎯 Key Features

### Separate Deployments
- **FirstApi** and **SecondApi** are deployed as independent applications
- Each has its own:
  - Deployment
  - Service
  - ServiceAccount
  - Optional Ingress
  - Optional HorizontalPodAutoscaler (HPA)

### OpenTelemetry Support
- Pre-configured OTLP environment variables:
  - `OTEL_EXPORTER_OTLP_ENDPOINT` - Collector endpoint
  - `OTEL_SERVICE_NAME` - Service identifier (first-api / second-api)
  - `OTEL_SERVICE_NAMESPACE` - Logical grouping
  - `OTEL_RESOURCE_ATTRIBUTES` - Environment and version tags

#### Example
- `OTEL_SERVICE_NAME`="videos-service" \
- `OTEL_EXPORTER_OTLP_ENDPOINT`="http://grafana-k8s-monitoring-alloy.default.svc.cluster.local:4318" \
- `OTEL_EXPORTER_OTLP_PROTOCOL`="http/protobuf" \
- `OTEL_RESOURCE_ATTRIBUTES`="service.namespace=default" \

### Environment Management
- Global environment setting (`dev`, `staging`, `prod`)
- Environment-specific value files
- Easy OTLP endpoint configuration per environment

### Production Ready
- Resource limits and requests
- Health checks (liveness and readiness probes)
- Horizontal Pod Autoscaling support
- Ingress configuration with TLS
- Security contexts and service accounts

## 🚀 Quick Start

### 1. Install with defaults
```bash
helm install otel-api ./helm/otel-api-poc
```

### 2. Install for development
```bash
helm install otel-api ./helm/otel-api-poc -f ./helm/otel-api-poc/values-dev.yaml
```

### 3. Install with custom OTLP endpoint
```bash
helm install otel-api ./helm/otel-api-poc \
  --set opentelemetry.endpoint="http://my-collector:4317" \
  --set global.environment="staging"
```

### 4. Access the APIs
```bash
# FirstApi
kubectl port-forward service/otel-api-otel-api-poc-firstapi 8080:80

# SecondApi
kubectl port-forward service/otel-api-otel-api-poc-secondapi 8081:80
```

## 📝 Configuration

### Global Settings
| Parameter | Description | Default |
|-----------|-------------|---------|
| `global.environment` | Environment name | `dev` |

### OpenTelemetry
| Parameter | Description | Default |
|-----------|-------------|---------|
| `opentelemetry.endpoint` | OTLP collector endpoint | `http://otel-collector:4317` |
| `opentelemetry.serviceNamespace` | Service namespace | `otel-api-poc` |

### Per API Settings (firstApi / secondApi)
| Parameter | Description | Default |
|-----------|-------------|---------|
| `*.enabled` | Enable/disable deployment | `true` |
| `*.replicaCount` | Number of replicas | `1` |
| `*.image.repository` | Docker image repository | `firstapi` / `secondapi` |
| `*.image.tag` | Image tag | `latest` |
| `*.service.type` | Service type | `ClusterIP` |
| `*.ingress.enabled` | Enable ingress | `false` |
| `*.autoscaling.enabled` | Enable HPA | `false` |
| `*.resources` | Resource limits/requests | `{}` |

## 📚 Documentation

- **[QUICKSTART.md](./QUICKSTART.md)** - Step-by-step deployment guide
- **[otel-api-poc/README.md](./otel-api-poc/README.md)** - Complete chart reference
- **[values.yaml](./otel-api-poc/values.yaml)** - All configurable values with comments

## 🔧 Common Operations

### Upgrade
```bash
helm upgrade otel-api ./helm/otel-api-poc -f ./helm/otel-api-poc/values-dev.yaml
```

### Uninstall
```bash
helm uninstall otel-api
```

### Validate
```bash
helm lint ./helm/otel-api-poc
```

### Preview changes
```bash
helm template otel-api ./helm/otel-api-poc
```

## 🎨 Customization Examples

### Deploy only FirstApi
```bash
helm install otel-api ./helm/otel-api-poc --set secondApi.enabled=false
```

### Enable autoscaling
```bash
helm install otel-api ./helm/otel-api-poc \
  --set firstApi.autoscaling.enabled=true \
  --set firstApi.autoscaling.minReplicas=2 \
  --set firstApi.autoscaling.maxReplicas=10
```

### Configure for different environments
```bash
# Development
helm install otel-api-dev ./helm/otel-api-poc \
  -f ./helm/otel-api-poc/values-dev.yaml \
  --namespace dev

# Production
helm install otel-api-prod ./helm/otel-api-poc \
  -f ./helm/otel-api-poc/values-prod.yaml \
  --namespace production
```

## 🐛 Troubleshooting

### Check deployment status
```bash
kubectl get pods -l app.kubernetes.io/instance=otel-api
helm status otel-api
```

### View logs
```bash
kubectl logs -l app.kubernetes.io/component=firstapi -f
kubectl logs -l app.kubernetes.io/component=secondapi -f
```

### Debug installation
```bash
helm install otel-api ./helm/otel-api-poc --dry-run --debug
```

## 📋 Requirements

- Kubernetes 1.19+
- Helm 3.2.0+
- Docker images for FirstApi and SecondApi
- OpenTelemetry Collector (optional but recommended)

## 🏗️ Architecture

```
┌─────────────────────────────────────────────┐
│           Kubernetes Cluster                │
│                                             │
│  ┌──────────────┐      ┌──────────────┐   │
│  │  FirstApi    │      │  SecondApi   │   │
│  │  Deployment  │      │  Deployment  │   │
│  │  (Separate)  │      │  (Separate)  │   │
│  └──────┬───────┘      └──────┬───────┘   │
│         │                     │            │
│  ┌──────▼───────┐      ┌──────▼───────┐   │
│  │  Service     │      │  Service     │   │
│  └──────┬───────┘      └──────┬───────┘   │
│         │                     │            │
│         └──────────┬──────────┘            │
│                    │                       │
│                    ▼                       │
│         ┌──────────────────┐               │
│         │ OTLP Collector   │               │
│         └──────────────────┘               │
└─────────────────────────────────────────────┘
```

## ✨ Features Highlights

✅ **Separate Apps**: FirstApi and SecondApi deployed independently  
✅ **OTLP Ready**: Pre-configured OpenTelemetry environment variables  
✅ **Multi-Environment**: Dev, staging, and prod configurations  
✅ **Production Ready**: HPA, ingress, resource limits, health checks  
✅ **Flexible**: Enable/disable each API independently  
✅ **Well Documented**: Comprehensive README and quick start guide  

## 📖 Next Steps

1. Review the [QUICKSTART.md](./QUICKSTART.md) for deployment instructions
2. Customize [values.yaml](./otel-api-poc/values.yaml) for your environment
3. Deploy to your Kubernetes cluster
4. Configure OpenTelemetry Collector for observability
5. Set up monitoring and alerting

---

**Chart Version**: 0.1.0  
**App Version**: 1.0.0  
**Maintained by**: OtelApiPoc Project

