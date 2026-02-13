{{/*
Expand the name of the chart.
*/}}
{{- define "otel-api-poc.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name for FirstApi.
*/}}
{{- define "otel-api-poc.firstApi.fullname" -}}
{{- if .Values.firstApi.fullnameOverride }}
{{- .Values.firstApi.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.firstApi.nameOverride }}
{{- if contains $name .Release.Name }}
{{- printf "%s-firstapi" .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s-firstapi" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create a default fully qualified app name for SecondApi.
*/}}
{{- define "otel-api-poc.secondApi.fullname" -}}
{{- if .Values.secondApi.fullnameOverride }}
{{- .Values.secondApi.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.secondApi.nameOverride }}
{{- if contains $name .Release.Name }}
{{- printf "%s-secondapi" .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s-secondapi" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "otel-api-poc.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels for FirstApi
*/}}
{{- define "otel-api-poc.firstApi.labels" -}}
helm.sh/chart: {{ include "otel-api-poc.chart" . }}
{{ include "otel-api-poc.firstApi.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels for FirstApi
*/}}
{{- define "otel-api-poc.firstApi.selectorLabels" -}}
app.kubernetes.io/name: {{ include "otel-api-poc.name" . }}-firstapi
app.kubernetes.io/instance: {{ .Release.Name }}
app.kubernetes.io/component: firstapi
{{- end }}

{{/*
Common labels for SecondApi
*/}}
{{- define "otel-api-poc.secondApi.labels" -}}
helm.sh/chart: {{ include "otel-api-poc.chart" . }}
{{ include "otel-api-poc.secondApi.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels for SecondApi
*/}}
{{- define "otel-api-poc.secondApi.selectorLabels" -}}
app.kubernetes.io/name: {{ include "otel-api-poc.name" . }}-secondapi
app.kubernetes.io/instance: {{ .Release.Name }}
app.kubernetes.io/component: secondapi
{{- end }}

{{/*
Create the name of the service account for FirstApi
*/}}
{{- define "otel-api-poc.firstApi.serviceAccountName" -}}
{{- if .Values.firstApi.serviceAccount.create }}
{{- default (include "otel-api-poc.firstApi.fullname" .) .Values.firstApi.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.firstApi.serviceAccount.name }}
{{- end }}
{{- end }}

{{/*
Create the name of the service account for SecondApi
*/}}
{{- define "otel-api-poc.secondApi.serviceAccountName" -}}
{{- if .Values.secondApi.serviceAccount.create }}
{{- default (include "otel-api-poc.secondApi.fullname" .) .Values.secondApi.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.secondApi.serviceAccount.name }}
{{- end }}
{{- end }}

