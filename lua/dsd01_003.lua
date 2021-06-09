-- Blaze Maiden, Rino

function NumberOfEffects()
	return 3
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1
		return q.Location, l.Deck, q.Name, "Trickstar"
	else if n == 2
		return q.Location, l.PlayerVC, q.Location, l.PlayerRC
	end
end


function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, l.TopSoul, false, false
	else if n == 2
		return a.OnAttack, l.PlayerVC, l.PlayerRC, true, true
	else if n == 3
		return a.OnBattleEnds, l.PlayerVC, l.PlayerRC, true, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsTopSoul() and obj.VanguardIs("Blaze Maiden, Reiyu") and obj.CanSuperiorCall(1) then
			return true
		end
	else if n == 2 then
		if obj.IsAttackingUnit() then
			return true
		end
	else if n == 3 then
		if obj.IsAttackingUnit() then
			return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.SuperiorCall(1)
	else if n == 2 then
		obj.AddPower(2, 2000)
	else if n == 3 then
		obj.AddPower(2, -2000)
	end
end