# Codex 작업 인수인계

> 학교 PC와 집 PC의 Codex가 같은 프로젝트 상태를 이어가기 위한 문서입니다.
> 새 대화를 시작할 때 Codex에게 이 파일을 먼저 읽고 실제 GitHub 상태를 확인하라고 요청하세요.

## Codex 시작 명령

새 Codex 대화에 아래 문장을 입력합니다.

```text
GitHub의 dragon1894-lab/moble2 저장소에서 dragon1894-lab 브랜치의
CODEX_HANDOFF.md를 먼저 읽어줘.
그다음 실제 브랜치와 최근 커밋 상태를 확인하고 작업을 이어가 줘.
내 허락 없이 브랜치를 합치거나 파일을 삭제하지 마.
```

## 저장소

- 저장소: https://github.com/dragon1894-lab/moble2
- 소유자: `dragon1894-lab`
- 프로젝트: Visual Studio 2022 WinForms 조별 프로젝트
- 공개 저장소 여부: 공개 저장소
- 기본 브랜치: `main`

## 브랜치 구조

### 공용 브랜치

- `main`: 테스트를 통과한 최종 완성본
- `test`: 개인 작업을 합쳐서 빌드하고 실행하는 통합 테스트용

### 개인 브랜치

1. `dragon1894-lab`
2. `ChoMinKyeong13`
3. `ChoiJH0711`
4. `DH3874`
5. `seunghee622`
6. `joyeongjun`

## 작업 흐름

1. 각 조원은 자기 이름 브랜치에만 코드를 올립니다.
2. 조원은 `main`과 `test`에 직접 올리지 않습니다.
3. 작업이 끝난 개인 브랜치를 `test`에 합칩니다.
4. `test`에서 충돌 해결, Visual Studio 빌드, 실행 테스트를 진행합니다.
5. 모든 기능이 정상 작동하면 `test`를 `main`에 합칩니다.
6. Pull Request를 만들 때 개인 작업 통합은 `base: test`, `compare: 개인 브랜치`로 설정합니다.
7. 최종 반영은 `base: main`, `compare: test`로 설정합니다.

## 업로드 규칙

### 올릴 파일

- `.sln`
- `.csproj`
- 모든 `.cs`
- 모든 `.Designer.cs`
- 모든 `.resx`
- 사용한 이미지, 아이콘, 리소스 폴더

### 올리지 않을 항목

- `.vs`
- `bin`
- `obj`
- `.csproj.user`

## 2026-08-10까지 완료된 작업

- 공개 저장소 `dragon1894-lab/moble2` 생성
- `main` 브랜치 및 초기 README 생성
- 공용 `test` 브랜치 생성
- 개인 이름 브랜치 6개 생성
- 개인 브랜치 6개의 README에 업로드·다운로드·실행·삭제 방법 작성
- `main`과 `test`의 README는 개인 안내문 업데이트에서 제외
- `dragon1894-lab` 브랜치에 두더지 게임 프로젝트 업로드 이력이 있음
  - `DDD.sln`
  - `DDD.csproj`
  - `Program.cs`
  - `GameForm.cs`
- 단, 2026-08-10 같은 날 이후 커밋에서 위 프로젝트 파일들이 삭제됨
- 아직 개인 브랜치들을 `test`에 통합하지 않음
- 아직 `test`를 `main`에 합치지 않음

## 2026-08-11 실제 GitHub 상태 확인

확인 기준: 2026-08-11 KST Codex 작업 중 GitHub API와 GitHub connector로 확인.

### 저장소 상태

- 저장소: `dragon1894-lab/moble2`
- 공개 저장소: 맞음
- 기본 브랜치: `main`
- 마지막 push 시각: 2026-08-10 14:29:18 UTC
- `main`과 `test`는 같은 커밋 `ac8fa2c2c2f0` 상태
- 협력자 목록 API는 인증이 필요해 공개 API만으로는 재확인하지 못함

### 브랜치별 최신 상태

| 브랜치 | HEAD | 마지막 커밋 | `test` 대비 | 현재 주요 파일 |
|---|---:|---|---:|---|
| `main` | `ac8fa2c2c2f0` | Initialize main branch with README | 동일 | `README.md` |
| `test` | `ac8fa2c2c2f0` | Initialize main branch with README | 동일 | `README.md` |
| `dragon1894-lab` | 인수인계 문서 갱신 커밋 | Update Codex handoff | 7 commits 이상 ahead | `README.md`, `CODEX_HANDOFF.md` |
| `ChoMinKyeong13` | `92dd4e91db3e` | Add team upload, download, run, and deletion guide | 1 commit ahead | `README.md` |
| `ChoiJH0711` | `2ac10c483547` | Add team upload, download, run, and deletion guide | 1 commit ahead | `README.md` |
| `DH3874` | `757023dbbace` | Add team upload, download, run, and deletion guide | 1 commit ahead | `README.md` |
| `seunghee622` | `ec95884aea1d` | Add team upload, download, run, and deletion guide | 1 commit ahead | `README.md` |
| `joyeongjun` | `54e8f9d66e75` | Add team upload, download, run, and deletion guide | 1 commit ahead | `README.md` |

### 업로드 상태 판단

- 2026-08-11 확인 당시 `.sln`, `.csproj`, `.cs`, `.Designer.cs`, `.resx` 파일이 남아 있는 개인 브랜치는 없음
- 모든 조원 브랜치의 현재 파일 구성은 안내용 `README.md` 중심임
- `dragon1894-lab` 브랜치도 현재는 `README.md`와 `CODEX_HANDOFF.md`만 남아 있음
- 따라서 아직 `test`에 통합할 실제 Visual Studio 프로젝트 작업물이 확인되지 않음

### 충돌 가능성 메모

- 현재 상태만 보면 개인 브랜치들이 모두 `README.md`를 수정했기 때문에 바로 합치면 README 충돌 가능성이 있음
- 프로젝트 파일이 아직 없으므로 WinForms 폼, `.csproj`, 리소스 중복 충돌은 판단할 수 없음
- `dragon1894-lab` 브랜치에는 프로젝트 파일 업로드 후 삭제 이력이 있으므로, 통합 전 삭제가 의도된 것인지 확인 필요

## 로컬 작업 상태: project_login

- 작업 경로: `C:\Users\USER\Documents\win\project_login\login`
- 기존 Visual Studio 2022 WinForms 프로젝트 `login.sln`을 사용함
- 2026-08-11에 SoundClound 로그인 기능 초안을 로컬에 구현함
  - 첫 폼: ID, PW, Login, Create account, Guest
  - 회원가입 폼: 이름, 생년월일, 전화번호, 아이디, 비밀번호, 닉네임, 개인정보 동의 라디오 버튼
  - 동의하지 않거나 미선택 시 “개인정보 수집에 동의 해주세요.” 메시지
  - 가입 후 첫 화면으로 돌아와 ID/PW 로그인 가능
  - 로그인/Guest 후 프로필과 Mixing, SoundCloud 버튼이 있는 완료 폼 표시
- 회원정보는 현재 실행 중에만 기억하는 초안이며 파일/DB 저장과 실제 미디어 재생은 아직 구현하지 않음
- `dotnet build login.sln --no-restore` 확인: 오류 0개, `net6.0-windows` 지원 종료 경고 1개
- 이 로컬 프로젝트는 아직 GitHub 브랜치에 업로드하거나 병합하지 않음

## 다음 작업

1. Visual Studio 2022에서 `C:\Users\USER\Documents\win\project_login\login\login.sln`을 열고 실제 폼 디자인을 다듬습니다.
2. 회원가입 정보를 프로그램 종료 후에도 보관할지(텍스트 파일/DB)를 결정합니다.
3. Mixing과 SoundCloud 버튼이 열 동작 또는 다음 폼을 정합니다.
4. 음원 재생이 필요하면 강의 16장의 Windows Media Player 또는 NAudio 예제를 기준으로 별도 구현합니다.
5. GitHub에 올리기 전 `.vs`, `bin`, `obj`, `.csproj.user`는 제외하고 소스와 프로젝트 파일만 확인합니다.
6. 조원들에게 각자 개인 브랜치에 실제 Visual Studio 프로젝트 파일을 다시 올렸는지 확인합니다.
7. 바로 합치지 말고 `test...개인브랜치` 비교로 변경 파일, 삭제 파일, 중복 폼, 프로젝트 파일 충돌 가능성을 먼저 보고합니다.
8. 사용자의 명시적 허락을 받은 뒤 개인 브랜치를 `test`에 통합합니다.

## Codex 작업 규칙

- GitHub의 실제 상태를 이 문서보다 우선합니다.
- 작업 시작 시 이 문서와 최근 커밋을 모두 확인합니다.
- 사용자 승인 없이 브랜치 병합, 파일 삭제, 협력자 삭제를 하지 않습니다.
- 충돌이 있으면 임의로 한쪽 코드를 버리지 말고 차이를 사용자에게 설명합니다.
- 작업을 마칠 때 이 문서의 **최근 작업 기록**과 **다음 작업**을 갱신합니다.

## 최근 작업 기록

### 2026-08-11

- `CODEX_HANDOFF.md`를 먼저 읽고 작업을 이어받음
- GitHub API로 실제 저장소, 브랜치, 최근 커밋 상태 확인
- 로컬 `git ls-remote`는 Windows schannel 자격 증명 오류로 실패하여 Python HTTPS API와 GitHub connector로 우회 확인
- 브랜치 8개 확인: `main`, `test`, 개인 브랜치 6개
- `main`과 `test`가 아직 동일 커밋임을 확인
- 모든 개인 브랜치에 현재 실제 Visual Studio 프로젝트 파일이 없고 README 중심 상태임을 확인
- `dragon1894-lab` 브랜치의 과거 프로젝트 파일 업로드 후 삭제 이력을 확인
- `C:\Users\USER\Documents\win\project_login\login`의 기존 `login.sln`에 SoundClound 로그인 기능 초안을 구현
- ch13~ch17 WinForms 강의 PDF를 검토하고 TextBox, RadioButton, GroupBox, Button Click, MessageBox, 여러 폼 전환 방식으로 작성
- 로컬 빌드 오류 0개 확인
- GitHub 병합이나 기존 파일 삭제는 수행하지 않음

### 2026-08-10

- 저장소, 협력자, 개인 브랜치 6개, `test`, `main` 구성 완료
- 조원용 README 안내문 배포 완료
- 학교 PC와 집 PC 사이의 Codex 인수인계를 위해 이 파일 생성

## 메모

이 파일은 Codex의 참고용입니다. 조원용 사용 방법은 각 개인 브랜치의 `README.md`를 확인하세요.
