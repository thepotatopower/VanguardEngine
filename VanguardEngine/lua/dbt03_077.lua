-- ディアブロスガールズ ベリンダ

function RegisterAbilities()
	-- on main phase
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnMainPhase)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsPlayerTurn()
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.Grade, 3, q.Other, o.GradeOrHigher})
end

function Activation()
	obj.SoulCharge(1)
	if obj.InFinalRush() then
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	end
end