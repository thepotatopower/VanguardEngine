-- 命を灯す幽けき光

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBlitzOrder)
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Tamayura")})
end

function Cost(check)
	if check then return obj.CanSB(2) end
	obj.SoulBlast(2)
end

function Activation()
	obj.Select({q.Location, l.PlayerVC, q.Count, 1})
	local id = obj.GetID({q.Location, l.Selected})
	if id >= 0 then
		local tempAbility = GiveAbility(GetID(), id)
		tempAbility.SetDescription(2)
		tempAbility.SetTiming(a.Cont)
		tempAbility.SetLocation(l.VC)
		tempAbility.SetActivation("TempAbility")
		tempAbility.SetResetTarget(id)
		tempAbility.SetResetTiming(p.UntilEndOfTurn)
	end
end

function TempAbility()
	obj.AddCardValue({q.Location, l.EnemyFrontRow}, cs.BonusPower, -10000, p.Continuous)
end