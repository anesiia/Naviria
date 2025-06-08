describe('Сторінка профілю', () => {
    beforeEach(() => {
        cy.loginAsMaria();
        cy.visit('/');
        cy.get('a[href="/profile"] img.avatar').click();
        cy.url().should('include', '/profile');
    });
    it('Має розділи Досягнення і Друзі з переліками, якщо вони є', () => {
        // Перевірка наявності заголовків
        cy.contains('Досягнення').should('exist');
        cy.contains('Друзі').should('exist');

        // Якщо є досягнення — перевіряємо наявність хоча б одного непорожнього
        cy.get('.achievements .achievement').then(($achievements) => {
            if ($achievements.length > 0) {
                cy.wrap($achievements).first().should('not.be.empty');
            }
        });

        // Якщо є друзі — перевіряємо, що є хоча б один непорожній
        cy.get('.friends .friend').then(($friends) => {
            if ($friends.length > 0) {
                cy.wrap($friends).first().should('not.be.empty');
            }
        });
    });


    it('Має шкалу рівня XP, олівець для редагування і опис або заглушку', () => {
        // Шкала XP має бути присутня
        cy.get('.level-info .scale .color-scale').should('exist');

        // Текст XP — має бути у форматі "123/456 xp"
        cy.get('.level-info').contains(/xp/).should('exist');

        // Олівець для редагування
        cy.get('.edit-profile img[alt="edit"]').should('exist');

        // Перевірка опису або заглушки
        cy.get('.description').then(($desc) => {
            const text = $desc.text().trim();
            expect(text.length).to.be.greaterThan(0);
            if (text === 'Опису ще нема, але ти можешь додати його у будь-який момент') {
                cy.get('.description').should('contain', 'Опису ще нема');
            } else {
                cy.get('.description').should('not.contain', 'Опису ще нема');
            }
        });
    });
});
