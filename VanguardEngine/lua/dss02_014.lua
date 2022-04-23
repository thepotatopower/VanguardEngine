-- 焔の密僧 カゲリ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnDiscard)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	obj.Select({q.Location, l.PlayerVC, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 10000, p.UntilEndOfTurn)
end