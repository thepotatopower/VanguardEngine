-- ブラザース・ソウル

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetActivation("Activation")
end

function Activation()
	obj.SoulCharge(2)
end