describe('Сторінка статистики', () => {
    beforeEach(() => {
        cy.loginAsMaria();
        cy.visit('/statistics');
    });

    it('Перевірка кругової діаграми при перемиканні режимів', () => {
        // 1. Перевіряємо, що діаграма завантажилась при відкритті сторінки (особиста статистика)
        cy.get('svg').should('exist');
        cy.get('svg').find('path').its('length').should('be.gt', 0);

        // 2. Перемикаємось на "Друзі"
        cy.contains('button', 'Друзі').should('not.be.disabled').click();
        cy.get('svg').should('exist');
        cy.get('svg').find('path').its('length').should('be.gt', 0);

        // 3. Перемикаємось на "Всі користувачі"
        cy.contains('button', 'Всі користувачі').should('not.be.disabled').click();
        cy.get('svg').should('exist');
        cy.get('svg').find('path').its('length').should('be.gt', 0);
    });

    it('Перевірка загальних статистик', () => {
        const stats = [
            'Користувачів',
            'Всього задач',
            'З них виконано',
            'Ви з нами вже',
            'Ми існуємо',
        ];
        stats.forEach((label) => {
            cy.contains('h4', label)
                .parent()
                .find('p')
                .invoke('text')
                .should('match', /\d+/);
        });
    });

    it('Перевірка таблиці лідерів', () => {
        cy.contains('h2', 'Таблиця лідерів');
        cy.get('table.leaderboard-table').should('exist');
        cy.get('table.leaderboard-table tbody tr').should('have.length.at.least', 1);
        cy.get('table.leaderboard-table tbody tr').each(($row) => {
            cy.wrap($row).find('td').should('have.length', 7);
        });
    });

    it('Перевірка лінійного графіка за місяцями', () => {
        cy.contains('h2', 'Статистика виконаних задач за місяцями');
        cy.get('svg').should('exist');
        cy.get('svg').find('path').its('length').should('be.gt', 0);
    });
});
