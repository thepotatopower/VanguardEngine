-- デザイアデビル ヒステラ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit()
end

function Condition()
	return obj.Exists({q.Location, l.PlayerRC, q.Other, o.Standing})
end

function Activation()
	local power
	if obj.GetPlayerDamage() >= 5 then
		power = 5000
	else
		power = 2000
	end
	local num = obj.GetNumberOf({q.Location, l.PlayerRC, q.Other, o.Standing})
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusPower, num * power, p.UntilEndOfTurn)
end