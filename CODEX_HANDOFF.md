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


### 2026-08-11 추가 수정

- 회원가입 폼을 Form2, 프로필 폼을 Form3으로 정리함
- 닉네임 입력과 표시는 제거하고 Form3 프로필에 가입 ID를 표시하도록 변경함
- Form2에 비밀번호 확인 TextBox를 추가함
- Continue 클릭 시 비밀번호가 다르면 “비밀번호를 다시 확인해 주세요.”를 표시함
- Mixing과 SoundCloud 버튼의 클릭 이벤트는 아직 만들지 않아 눌러도 동작하지 않음
- 로컬 `login.sln` 빌드: 오류 0개, net6.0-windows 지원 종료 경고 1개


### 2026-08-11 디자이너 전환

- Form1, Form2, Form3의 컨트롤 선언과 배치를 각 `.Designer.cs` 파일로 옮김
- 세 폼에 기본 생성자를 추가해 Visual Studio WinForms 디자이너에서 폼을 열 수 있게 함
- 이제 컨트롤이 실행 시에만 생성되지 않고 디자이너 화면에도 표시됨
- 로컬 `login.sln` 빌드: 오류 0개, net6.0-windows 지원 종료 경고 1개

---

## 2026-08-14 최신 인수인계

### 현재 기준 브랜치와 프로젝트

- 실제 최신 통합 코드는 GitHub `DH3874` 브랜치에 있음
- `DH3874`는 확인 시점에 `main`보다 4개 커밋 앞서 있음
- 이전 통합본 `soundkloud` 폴더도 남아 있으나, 앞으로 확인하고 수정할 최신 프로젝트는 아래 경로임

```text
DH3874/DJing Final/DJing.sln
```

- 프로그램 시작점은 `DJing Final/Program.cs`이고 `Application.Run(new Login())`으로 로그인 창을 실행함
- 대상 프레임워크는 `.NET 6 Windows`
- 주요 패키지:
  - `MySql.Data 26.7.0`
  - `NAudio 2.3.0`
  - `SoundTouch.Net 2.3.2`
- 프로그램이 사용하는 DB는 `djing`
- 현재 DB 연결 문자열은 여러 파일에 직접 들어 있으며 기본값은 다음과 같음

```text
Server=localhost;Database=djing;Uid=root;Pwd=1111;
```

### dragon1894-lab의 로그인 원본 작업

- DB 연결 전 로그인 프로젝트가 다음 경로에 보존되어 있음

```text
dragon1894-lab/login1_0811
```

- 로컬 원본 경로:

```text
C:\Users\moble\Documents\Codex\Mobel_project_login_0812\login1_0811
```

- GitHub `login1_0811`과 로컬 원본은 핵심 `Form1.cs`~`Form4.cs` 동작 코드가 같은 버전임
- `Form1`: DB 없이 `MyID`, `MyPW`로 로그인
- `Form2`: 회원가입 정보 검증 및 `CreatedID`, `CreatedPW` 전달
- `Form3`: 로그인 후 선택 화면 및 프로필 수정 폼 연결
- `Form4`: 프로필 아이콘 선택, 비밀번호 변경, 전화번호 변경 UI와 입력 검증
- `login1_0813`은 이후 MySQL 연동이 추가된 버전
- `DH3874/DJing Final`은 팀 프로젝트 최신 통합본

### 최신 DJing Final 기능 확인

현재 확인된 기능:

- 로그인, 회원가입, 관리자 분기, 게스트 로그인
- 프로필 아이콘, 비밀번호, 전화번호 변경
- 음악 업로드, 수정, 삭제
- 음악 검색, 재생, 이전/다음 곡, 볼륨, 파형
- 음악 좋아요와 재생목록
- 댓글 작성, 삭제, 댓글 좋아요
- 마이페이지
- DJ 믹싱 화면과 오디오 처리 기능

### 최신 통합본 점검 결과

1. 여러 폼의 세로 중앙 배치 계산이 잘못되어 있음.

```csharp
this.Top + (this.Height - childForm.Height) / this.Top
```

위 코드는 `this.Top == 0`이면 오류가 날 수 있으므로 아래처럼 수정 필요.

```csharp
this.Top + (this.Height - childForm.Height) / 2
```

2. DB 이름과 SQL 파일이 불일치함.
   - C# 코드는 `djing` DB 사용
   - 루트 `soundcloud_db.sql`은 `soundcloud_db` 생성
   - `DJing Final/testing.sql`에서 `member`, `songs`, `song_likes`, `playlist_songs` 생성문은 주석 상태
   - 새 PC에서 한 번에 설치할 수 있는 통합 DB 생성 SQL이 필요함

3. SQL 초안의 `songs.user_id`에 `UNIQUE`가 있어 그대로 생성하면 사용자 한 명이 곡 하나만 업로드할 수 있음. 여러 곡 업로드를 위해 `UNIQUE` 제거 필요.

4. 관리자 음악 삭제 시 `playlist_songs`, `song_likes`, `songs`만 삭제함. 해당 곡에 댓글이 있으면 `comment.SongId` 외래키 때문에 삭제가 실패할 수 있으므로 댓글 관련 데이터 처리 필요.

5. 댓글 프로필 아이콘을 표시할 때 댓글마다 `new ProfileEdit(...)`를 생성함. `ProfileEdit` 생성자는 DB 조회까지 수행하므로 댓글이 많을수록 불필요한 폼 생성과 DB 조회가 반복됨. 아이콘은 공용 리소스 함수로 직접 읽도록 개선 필요.

6. DB 연결 문자열과 root 비밀번호가 여러 파일에 중복되어 있음. 공용 설정 또는 DB 헬퍼로 분리 필요.

7. 비밀번호가 평문으로 저장되고 비교됨. 학교 과제 시연은 가능하지만 실제 서비스 기준으로는 해시 저장 필요.

8. 일반 로그인 성공 시 `UserSession.IsGuest = false` 초기화를 넣는 것이 안전함.

9. `Choice.UpdateProfileIconToDB()`는 현재 호출되지 않는 코드임.

10. 저장소에 `soundkloud`와 `DJing Final`이 동시에 있으므로 잘못된 솔루션을 수정하지 않도록 주의. 이후 작업 대상은 반드시 `DJing Final`.

### 아이디/비밀번호 찾기 구현 계획

- 현재 최신 `DJing Final/Login`에는 다음 버튼만 있음
  - 로그인
  - 회원가입
  - 게스트로 시작
- 아이디/비밀번호 찾기 기능과 `FindAccount` 폼은 아직 없음
- 다음 작업은 `DH3874/DJing Final`을 기준으로 진행
- 새로 추가할 파일:

```text
DJing Final/FindAccount.cs
DJing Final/FindAccount.Designer.cs
DJing Final/FindAccount.resx
```

- `Login` 폼에 `bt_FindAccount` 버튼 추가
- 버튼 클릭 시 `FindAccount.ShowDialog(this)` 실행

아이디 찾기 방식:

```sql
SELECT user_id
FROM member
WHERE name = @name
  AND phone = @phone
LIMIT 1;
```

비밀번호 찾기는 기존 비밀번호를 표시하지 않고, 아이디·이름·전화번호가 일치하면 새 비밀번호로 재설정:

```sql
UPDATE member
SET password = @newPassword
WHERE user_id = @userId
  AND name = @name
  AND phone = @phone;
```

예정된 FindAccount 컨트롤 이름:

```text
아이디 찾기:
tb_FindName
tb_FindPhone
lb_FindIDResult
bt_FindID

비밀번호 재설정:
tb_ResetID
tb_ResetName
tb_ResetPhone
tb_NewPW
tb_NewPWCheck
bt_ResetPW
```

- SQL은 반드시 매개변수 `@name`, `@phone`, `@userId`, `@newPassword`를 사용
- 조회 결과는 없을 수 있으므로 `object? result = cmd.ExecuteScalar();` 사용 가능
- `object?`의 `?`는 해당 변수에 `null`이 들어갈 수 있다는 의미
- 현재 프로젝트는 `<Nullable>enable</Nullable>`이므로 nullable 문법 사용 가능
- 과제용으로 이름과 전화번호를 본인 확인 수단으로 사용하되 실제 서비스라면 이메일 또는 문자 인증이 필요함

### 다음 작업 우선순위

1. `DH3874/DJing Final`을 내려받아 Visual Studio에서 빌드 확인
2. 로그인 창에 아이디/비밀번호 찾기 버튼 배치
3. `FindAccount` 폼 제작
4. 아이디 찾기와 비밀번호 재설정 로직 연결
5. 테스트용 회원정보로 성공·실패·빈칸·비밀번호 불일치 확인
6. 위 통합본 점검 문제 중 창 위치 계산과 DB 스키마 불일치를 우선 수정
7. 사용자 승인 없이 `DH3874`, `test`, `main` 브랜치를 병합하거나 기존 파일을 삭제하지 않음
