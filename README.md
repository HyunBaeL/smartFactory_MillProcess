# smartFactory_MillProcess
스마트팩토리 압연공정 시뮬레이터
# 🏭 압연공정 시뮬레이터 (Rolling Process Simulator)

본 프로젝트는 스마트팩토리의 핵심 공정 중 하나인 **압연(Rolling) 공정**을 시뮬레이션하는 데스크탑 애플리케이션입니다.  
`C# WPF`, `MVVM Toolkit`, `LiveCharts2`를 활용하여 시각화 중심의 실시간 데이터 처리와 그래프 분석 기능을 구현하였습니다.

---

## 👨‍💻 팀 구성

| 이름 | 역할 |
|------|------|
| 이현배 (리더) | 프로젝트 총괄, 데이터베이스 설계, UI 개발, LiveCharts2 그래프 구현, 데이터 처리, MVVM 로직 구성 |
| 서창범 | 공정 계산 로직 구현, 압연공정 UI 구현 |
| 유현수 | 공정 계산 로직 구현, 압연공정 UI 구현 |

---

## 🔧 사용 기술 스택

- **언어/플랫폼**: C# (.NET 6 or .NET Framework)
- **UI 프레임워크**: WPF (Windows Presentation Foundation)
- **디자인 패턴**: MVVM (Model-View-ViewModel)
- **MVVM 지원**: CommunityToolkit.Mvvm
- **데이터 시각화**: LiveCharts2
- **PLC 통신**: TCP/IP 기반 시뮬레이션

---

## 💡 주요 기능

- 📊 **압연 공정 작업량 그래프 시각화**  
  → LiveCharts2로 시간별 작업량을 실시간 표시

- 🧠 **MVVM 아키텍처 설계**  
  → 유지보수성과 확장성을 고려한 구조 설계

- ⏱ **공정 시간 및 생산량 분석**  
  → Today 기준 누적 작업량, 기준 대비 그래프 비교

---
