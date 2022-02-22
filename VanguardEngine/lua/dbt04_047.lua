-- 闇冥に熔けこ、其の邪心

function RegisterAbilities()
	-- on order
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBlitzOrder)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	local count = obj.GetNumberOf({q.Location, l.PlayerUnits, q.Name, obj.GetNameFromCardID("dbt01_t01"), q.UnitType, u.Token})
	obj.Select({q.Location, l.PlayerUnits, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, count * 10000, p.UntilEndOfBattle)
end