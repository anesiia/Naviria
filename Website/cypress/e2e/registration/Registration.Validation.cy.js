describe('Сторінка реєстрації – лише негативні кейси', () => {
  const validUser = {
    name: 'Лайт',
    surname: 'Ягамі',
    email: 'light.yagami@example.com',
    password: 'Qwerty123',
    nickname: 'Kira3000',
    gender: 'm',
    birthDate: '2000-01-01',
  };

  beforeEach(() => {
    cy.visit('/registration');
  });

  const fillForm = (user = validUser, passwordCheck = user.password) => {
    cy.get('input[name="name"]').clear().type(user.name);
    cy.get('input[name="surname"]').clear().type(user.surname);
    cy.get('input[name="email"]').clear().type(user.email);
    cy.get('input[name="password"]').clear().type(user.password);
    cy.get('input[name="repeat-password"]').clear().type(passwordCheck);

    if (user.gender) {
      cy.get(`input[type="radio"][value="${user.gender}"]`).check({ force: true });
    }

    if (user.birthDate) {
      cy.get('input[name="birth-date"]').clear().type(user.birthDate);
    } else {
      cy.get('input[name="birth-date"]').clear();
    }

    cy.get('input[name="nickname"]').clear().type(user.nickname);
  };

  describe('Помилки перевірки', () => {
    const testCases = [
      {
        description: 'некоректні символи в імені',
        user: { ...validUser, name: '@!$' },
        error: "Ім'я введено некоректно",
      },
      {
        description: 'некоректні символи в прізвищі',
        user: { ...validUser, surname: '123' },
        error: 'Прізвище введено некоректно',
      },
      {
        description: 'невірний формат електронної пошти',
        emails: ['example.com', 'example@', 'example@com', '@example.com'],
        error: 'Невірний формат пошти',
      },
      {
        description: 'слабкий пароль – лише літери',
        user: { ...validUser, password: 'Password' },
        passwordCheck: 'Password',
        error: 'Пароль має містити великі та малі літери та цифру',
      },
      {
        description: 'слабкий пароль – лише цифри',
        user: { ...validUser, password: '12345678' },
        passwordCheck: '12345678',
        error: 'Пароль має містити великі та малі літери та цифру',
      },
      {
        description: 'слабкий пароль – лише малі літери',
        user: { ...validUser, password: 'abc' },
        passwordCheck: 'abc',
        error: 'Пароль має містити великі та малі літери та цифру',
      },
      {
        description: 'паролі не збігаються',
        user: { ...validUser, password: 'Qwerty123' },
        passwordCheck: 'Pass5678',
        error: 'Паролі не збігаються',
      },
      {
        description: 'не вказано стать',
        user: { ...validUser, gender: '' },
        error: 'Оберіть стать',
      },
      {
        description: 'користувач надто молодий (1 рік)',
        user: { ...validUser, birthDate: `${new Date().getFullYear() - 1}-01-01` },
        error: 'Потрібно бути старше 18 років',
      },
      {
        description: 'нікнейм містить пробіли або символи',
        user: { ...validUser, nickname: 'nickname with space' },
        error: 'Нікнейм має містити лише латинські літери та цифри (3-20 символів)',
      },
      {
        description: 'нікнейм кирилицею',
        user: { ...validUser, nickname: 'ім’я' },
        error: 'Нікнейм має містити лише латинські літери та цифри (3-20 символів)',
      },
      {
        description: 'нікнейм занадто короткий',
        user: { ...validUser, nickname: 'te' },
        error: 'Нікнейм має містити лише латинські літери та цифри (3-20 символів)',
      },
      {
        description: 'нікнейм занадто довгий (понад 20 символів)',
        user: { ...validUser, nickname: '123456789012345678901' },
        error: null, // Якщо помилки не очікується
        customCheck: () => {
          cy.get('input[name="nickname"]')
              .invoke('val')
              .should('have.length', 20);
        }
      },

    ];

    testCases.forEach(({ description, user, passwordCheck, emails, error, customCheck }) => {
      if (emails) {
        emails.forEach((email) => {
          it(`має показувати помилку для ${description} - "${email}"`, () => {
            fillForm({ ...validUser, email });
            cy.get('.submit-button').click();
            cy.contains(error).should('be.visible');
          });
        });
      } else if (customCheck) {
        it(`слід виконати спеціальну перевірку для ${description}`, () => {
          fillForm(user, passwordCheck);
          cy.get('.submit-button').click();
          customCheck();
        });
      } else {
        it(`має показувати помилку для ${description}`, () => {
          fillForm(user, passwordCheck);
          cy.get('.submit-button').click();
          cy.contains(error).should('be.visible');
        });
      }
    });
  });
});
