-- 歌声は夜空に響いて リュドミラ

function RegisterAbilities()
	-- on end of battle
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBattleEnds)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsBooster()
end

function Cost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function CanFullyResolve()
	return obj.GetNumberOf("RomanticFilter", {l.SungThisTurn}) > 0 
	or obj.GetNumberOf("TwilightFilter", {l.SungThisTurn}) > 0
	or obj.GetNumberOf("MadderFilter", {l.SungThisTurn}) > 0
	or obj.GetNumberOf("FractaleFilter", {l.SungThisTurn} > 0)
end

function RomanticFilter(snapshot)
	return snapshot.name == obj.GetNameFromCardID("dlbt01_120")
end

function TwilightFilter(snapshot)
	return snapshot.name == obj.GetNameFromCardID("dlbt01_057")
end

function MadderFilter(snapshot)
	return snapshot.name == obj.GetNameFromCardID("dlbt01_056")
end

function FractaleFilter(snapshot)
	return snapshot.name == obj.GetNameFromCardID("dlbt01_022")
end

function Activation()
	if obj.GetNumberOf("RomanticFilter", {l.SungThisTurn}) > 0 then
		obj.Select({q.Location, l.PlayerUnits, q.Count, 1})
		obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	end
	if obj.GetNumberOf("TwilightFilter", {l.SungThisTurn}) > 0 then
		obj.LookAtTopOfDeck(1)
	end
	if obj.GetNumberOf("MadderFilter", {l.SungThisTurn}) > 0 and obj.IsSameZone() and obj.YesNo(obj.GetDescription(2)) then
		obj.AddToHand({q.Other, o.ThisFieldID})
	end
	if obj.GetNumberOf("FractaleFilter", {l.SungThisTurn}) > 0 and obj.CanPayCost("AdditionalCost") and obj.PayAdditionalCost() then
		obj.Draw(1)
	end
end

function AdditionalCost(check)
	if check then return obj.CanDiscard(1) end
	obj.Discard(1)
end

