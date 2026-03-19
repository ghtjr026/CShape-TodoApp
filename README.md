# 📝 TodoApp — 할일 관리 애플리케이션

WPF 기반의 할일 관리 데스크톱 애플리케이션입니다.  
**클린 아키텍처** 원칙에 따라 레이어를 분리하여 설계한 토이 프로젝트입니다.

---

## 🛠️ 기술 스택

| 항목 | 내용 |
|------|------|
| **프레임워크** | .NET Framework 4.8 |
| **UI** | WPF (MVVM 패턴) |
| **언어** | C# 7.3 |
| **데이터 저장** | JSON 파일 (DataContractJsonSerializer) |
| **외부 패키지** | 없음 (프레임워크 기본 제공 기능만 사용) |

---

## 📁 아키텍처 구조

```
TodoApp/
├── Presentation          ─── WPF UI 레이어
│   ├── Views/                  MainWindow.xaml (화면 레이아웃)
│   ├── ViewModel/              MainViewModel, TodoListViewModel,
│   │                           TodoItemViewModel, TodoEditorViewModel,
│   │                           ViewModelBase
│   ├── Commands/               RelayCommand (ICommand 구현)
│   └── Converters/             BoolToStrikethroughConverter,
│                               PriorityToColorConverter
│
├── AppCore               ─── 애플리케이션 기능 흐름 레이어
│   ├── Interfaces/             ITodoRepository
│   └── UseCases/               CreateTodoUseCase, LoadTodosUseCase,
│                               UpdateTodoUseCase, DeleteTodoUseCase,
│                               ToggleTodoStatusUseCase, SaveTodosUseCase
│
├── Domain                ─── 핵심 비즈니스 규칙 레이어
│   ├── Entities/               TodoTask
│   └── Enums/                  TodoPriority, TodoStatus
│
├── Infrastructure        ─── 외부 시스템 연동 레이어
│   └── Persistence/            JsonTodoRepository
│
├── Shared                ─── 공통 모듈
│   └── Result/                 Result, Result<T>
│
└── App.xaml / App.xaml.cs ─── Composition Root (의존성 조립)
```

---

## 🏗️ 아키텍처 구조 설명

### 의존성 방향

```
Presentation → AppCore → Domain
                 ↑
            Infrastructure
               ↑
             Shared
```

상위 레이어가 하위 레이어를 참조하며, **Infrastructure는 AppCore의 인터페이스를 구현**하되 AppCore가 Infrastructure를 직접 참조하지 않습니다 (의존성 역전 원칙).

### 각 레이어의 역할

| 레이어 | 역할 | 핵심 원칙 |
|--------|------|-----------|
| **Presentation** | 화면 표시와 사용자 입력 처리. MVVM 패턴으로 View와 로직을 분리 | View는 ViewModel만 알고, ViewModel은 UseCase만 호출 |
| **AppCore** | 앱의 기능 흐름(UseCase) 정의. 하나의 UseCase는 하나의 기능만 담당 | 저장 방식을 모름 — 인터페이스(ITodoRepository)로만 접근 |
| **Domain** | 핵심 비즈니스 규칙 보유. 엔티티가 자신의 상태 변경 규칙을 메서드로 관리 | 외부 의존성 없음 — 순수 C# 클래스 |
| **Infrastructure** | 파일 저장, DB 연결 등 외부 시스템과의 통신 담당 | AppCore 인터페이스의 구현체 제공 |
| **Shared** | 여러 레이어에서 공통으로 사용하는 기반 타입 | Result 패턴으로 예외 대신 결과값 반환 |

### Composition Root (`App.xaml.cs`)

앱 시작 시 **수동 DI**로 모든 의존성을 조립합니다.

```
Repository 생성 → UseCase에 주입 → ViewModel에 주입 → Window에 바인딩
```

---

## ⚡ 기능 설명

### 1. 할일 추가

- 제목, 설명, 우선순위, 마감일을 입력하여 새 할일을 생성합니다.
- 제목이 비어있으면 오류 메시지를 표시합니다.
- `CreateTodoUseCase` → `TodoTask` 엔티티 생성 → JSON 파일에 저장.

### 2. 할일 수정

- 목록에서 항목을 선택 → **[선택 항목 편집]** 버튼 → 편집 폼에 데이터 로드.
- 내용 변경 후 **[수정 저장]** 버튼으로 반영합니다.
- `UpdateTodoUseCase` → 도메인 메서드(`UpdateTitle`, `UpdatePriority` 등)로 변경.

### 3. 할일 삭제

- 목록에서 항목 선택 → **[삭제]** 버튼으로 제거합니다.
- `DeleteTodoUseCase` → ID로 찾아 목록에서 제거 → 저장.

### 4. 완료 체크

- 목록의 체크박스를 클릭하면 완료/미완료 상태가 토글됩니다.
- 완료된 항목은 **취소선**으로 표시됩니다.
- `ToggleTodoStatusUseCase` → `TodoTask.ToggleStatus()` 호출.

### 5. 우선순위

- **High**(빨강), **Medium**(주황), **Low**(회색) 3단계로 구분됩니다.
- 목록에서 색상 원으로 시각적으로 구분합니다.

### 6. 마감일

- 선택적으로 마감일을 설정할 수 있습니다.
- 미완료 상태에서 마감일이 지나면 **"⚠ 마감 초과!"** 경고가 표시됩니다.

### 7. 완료/미완료 필터

- **[전체]** / **[미완료]** / **[완료]** 버튼으로 목록을 필터링합니다.
- WPF `ICollectionView.Filter`를 활용한 실시간 필터링.

---

## 📂 데이터 저장

- 실행 폴더의 `todos.json` 파일에 자동 저장됩니다.
- `DataContractJsonSerializer`를 사용하여 직렬화/역직렬화합니다.
- 앱 시작 시 자동으로 기존 데이터를 로드합니다.

---

## 🚀 실행 방법

1. Visual Studio 2022 이상에서 `TodoApp.slnx`를 엽니다.
2. 빌드 대상이 **Debug / AnyCPU**인지 확인합니다.
3. `F5`를 눌러 실행합니다.

---

## 📌 프로젝트 구조 설계 의도

- **토이 프로젝트이지만 실무 아키텍처 패턴을 학습하기 위해** 레이어를 의도적으로 분리했습니다.
- 외부 패키지 없이 .NET Framework 기본 제공 기능만으로 구현하여 **프레임워크 이해에 집중**합니다.
- `Result` 패턴으로 예외 대신 결과값을 반환하여 **에러 처리 흐름을 명시적으로** 만들었습니다.
