-- うぇいきー・わーるど エリサ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.IsBooster()
end

function Filter(snapshot)
	return snapshot.name == obj.GetNameFromCardID("dlbt01_120")
end

function Condition()
	return obj.GetNumberOf("Filter", {l.SungThisTurn}) > 0
end

function Activation()
	obj.SoulCharge(1)
end