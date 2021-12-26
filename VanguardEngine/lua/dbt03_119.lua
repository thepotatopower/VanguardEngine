-- 遥かなる理想求めて

function RegisterAbilities()
	-- order
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.OnOrder)
	ability1.SetActivation("Activation")
end

function Activation()
	obj.Select({q.Location, l.PlayerUnits, q.Count, 1})
	if obj.Exists({q.Location, l.Selected}) then
		local target = obj.GetID({q.Location, l.Selected})
		local given = GiveAbility(GetID(), target)
		given.SetTiming(a.OnAttack)
		given.SetLocation(l.VC)
		given.SetLocation(l.RC)
		given.SetTrigger("OnAttackTrigger")
		given.SetCondition("OnAttackCondition")
		given.SetActivation("OnAttack")
		given.SetResetTarget(target)
		given.SetRestTiming(p.UntilEndOfTurn)
	end
end

function OnAttackTrigger()
	return obj.Exists({q.Location, l.PlayerUnits, q.Other, o.Attacking, q.Other, o.NotThisFieldID})
end

function OnAttackCondition()
	return obj.Exists({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID})
end

function OnAttack()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end