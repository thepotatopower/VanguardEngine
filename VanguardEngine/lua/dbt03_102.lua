-- 天穿の騎士 エザルタ

function RegisterAbilities()
	-- on place / on discard
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC, a.OnDiscard)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	if obj.IsApplicable() and obj.GetCurrentActivation() == a.OnDiscard then
		return obj.IsPlayerTurn()
	end
	return obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.BackRowRC, q.Other, o.Resting})
end

function Activation()
	obj.ChooseStand({q.Location, l.BackRowRC, q.Other, o.Resting, q.Count, 1})
end
