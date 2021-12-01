-- ディアブロスマドンナ メリル

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit()
end

function Activation()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusPower, 2000, p.UntilEndOfTurn)
	if obj.InFinalRush() and obj.CanSB(1) and obj.YesNo(obj.GetDescription(2)) then
		obj.Select({q.Location, l.PlayerVC, q.Count, 1})
		obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 10000, p.UntilEndOfTurn)
	end
end