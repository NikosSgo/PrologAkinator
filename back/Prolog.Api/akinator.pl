:- encoding(utf8).

:- dynamic question/3.

:- use_module(library(http/http_json)).
:- use_module(library(http/thread_httpd)).
:- use_module(library(http/http_dispatch)).
:- use_module(library(http/json)). 
:- use_module(library(http/http_open)).
:- use_module(library(http/http_cors)).

:- http_handler(root(akinator), akinator_handler, []).
:- http_handler(root(first_question), first_question_handler, []).
:- http_handler(root(next_question), next_question_handler, []).
:- http_handler(root(question), add_question_handler, []).


% Обработчик для получения следующего вопроса
next_question_handler(Request) :-
    cors_enable(Request, [methods([get,post,options])]),
    http_read_json_dict(Request, DictIn),
    get_dict(asked_questions, DictIn, AskedQuestions),
    (   get_next_question(AskedQuestions, NextQuestion)
    ->  reply_json_dict(_{next_question: NextQuestion})
    ;   reply_json_dict(_{next_question: null}) % если больше нет вопросов
    ).

% Для добавления вопроса через JSON
add_question_handler(Request) :-
    cors_enable(Request, [methods([post,options])]),
    http_read_json_dict(Request, DictIn),
    get_dict(object, DictIn, Object),
    get_dict(question, DictIn, Question),
    get_dict(answer, DictIn, Answer),
    convert_answer(Answer, ConvertedAnswer),
    (   question(Object, Question, ConvertedAnswer)
    ->  reply_json_dict(_{status: "already exists"})
    ;   assertz(question(Object, Question, ConvertedAnswer)),
        reply_json_dict(_{status: "added"})
    ).

% Получение следующего вопроса, которого еще не было в AskedQuestions
get_next_question(AskedQuestions, NextQuestion) :-
    findall(Q, question(_, Q, _), AllQuestions),
    subtract(AllQuestions, AskedQuestions, RemainingQuestions),
    RemainingQuestions = [NextQuestion|_]. % берем первый из оставшихся

first_question("Игрок в настоящее время выступает в одной из топ-5 европейских лиг?").

first_question_handler(Request) :-
    cors_enable(Request, [methods([get,post,options])]),
    first_question(Question),
    reply_json_dict(_{question: Question}).

% URL, откуда брать факты при запуске
facts_source_url('http://localhost:5000/facts').

% Преобразование ответа из 1/0 в "Да"/"Нет"
convert_answer(Answer, Converted) :-
    (Answer = 1 -> Converted = "Да";
     Answer = 0 -> Converted = "Нет";
     Converted = Answer).

% Запуск сервера с инициализацией фактов
start_server(Port) :-
    facts_source_url(URL),
    format('Загружаем факты с ~w ...~n', [URL]),
    catch(
        update_questions_from_remote(URL),
        E,
        (
            print_message(error, E),
            format('Ошибка загрузки фактов: ~w~n', [E]),
            format('Продолжаем без фактов...~n'),
            fail
        )
    ),
    format('Факты загружены.~n'),
    http_server(http_dispatch, [port(Port)]).

% Загрузка и обновление фактов из JSON массива с удалённого сервера
update_questions_from_remote(URL) :-
    retractall(question(_,_,_)),
    setup_call_cleanup(
        http_open(URL, In, []),
        (
            json_read_dict(In, JsonArray),
            length(JsonArray, Len),
            format('Получено ~d фактов~n', [Len]),
            forall(
                member(Dict, JsonArray),
                (
                    get_dict(answer, Dict, Answer),
                    get_dict(object, Dict, Object),
                    get_dict(question, Dict, Question),
                    convert_answer(Answer, ConvertedAnswer),
                    assertz(question(Object, Question, ConvertedAnswer))
                )
            )
        ),
        close(In)
    ).

% Игровой цикл
akinator_handler(Request) :-
    cors_enable(Request, [methods([get,post,options])]),
    http_read_json_dict(Request, DictIn),
    get_dict(answers, DictIn, AnswersList),
    process_answers(AnswersList, Objects),
    reply_json_dict(_{result: Objects}).

process_answers(Answers, FilteredObjects) :-
    findall(Object, question(Object, _, _), ObjectsDup),
    sort(ObjectsDup, Objects),
    filter_objects(Objects, Answers, FilteredObjects).

% Если список ответов пустой — возвращаем текущий список объектов
filter_objects(Objects, [], Objects).

% Рекурсивно фильтруем объекты по ответам
filter_objects(CurrentObjects, [AnswerDict|RestAnswers], Filtered) :-
    Question = AnswerDict.question,
    UserAnswer = AnswerDict.answer,
    % Оставляем только объекты, которые подходят под текущий ответ
    findall(Object,
        (member(Object, CurrentObjects), question(Object, Question, UserAnswer)),
        FilteredObjects),
    filter_objects(FilteredObjects, RestAnswers, Filtered).

print_all_questions :-
    forall(question(O,Q,A),
           format('Fact: ~w | ~w | ~w~n', [O,Q,A])).

:- initialization(start_server(6000)).