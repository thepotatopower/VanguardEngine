-- 紫青の幻蝶 ファンジュ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.PlayerUnits, q.Count, 5})
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	obj.SoulCharge(3)
	if obj.SoulCount() >= 10 then
		obj.Select({q.Location, l.PlayerVC, q.Count, 1})
		obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	end
end