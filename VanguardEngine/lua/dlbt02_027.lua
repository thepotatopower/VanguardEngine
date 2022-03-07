-- あたふたパニック フローチェ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Cont")
end

function Cont()
	return obj.GetNumberOf("Filter", {l.SungThisTurn}) > 0
end

function Filter(snapshot)
	snapshot.name == obj.GetNameFromCardID("dlbt01_022")
end