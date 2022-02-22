-- 樹角獣 アルヴァン

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.BackRowRC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.IsAttackingUnit() then
		obj.AddPlayerValue(ps.GuardRestrict, 2, p.UntilEndOfBattle, false)
	end
end