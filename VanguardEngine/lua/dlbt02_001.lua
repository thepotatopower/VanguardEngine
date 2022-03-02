-- Astesice×Live カイリ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.VC)
	ability1.SetActivation("Cont")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.VC)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCondition("OnAttackCondition")
	ability2.SetActivation("OnAttack")
end

function Cont()
	obj.AddCardValue({q.Location, l.PlayerRC, q.CardState, cs.PlacedThisTurn}, cs.BonusPower, 5000, p.Continuous)
end

function OnAttackTrigger()
	return obj.IsApplicable()
end

function OnAttackCondition()
	return obj.Exists({q.Location, l.Soul, q.NameContains, obj.LoadName("Kairi")})
end

function OnAttack()
	obj.ChooseAddToHand({q.Location, l.PlayerRC, q.Count, 2, q.Min, 0})
	obj.SuperiorCall({q.Location, l.Hand, q.Count, 2})
end