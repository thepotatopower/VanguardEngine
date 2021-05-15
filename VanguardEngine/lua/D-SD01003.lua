-- Blaze Maiden, Rino

function NumberOfEffects()
	return 2
end

function NumberOfParams(n)
	if n == 1
		return 1
	else if n == 2
		return 1
	end
end

function GetParam(n, m)
	if n == 1
		if m == 1
			return q.Location, l.Deck, q.Name, "Trickstar"
		end
	else if n == 2
		if m == 1
			return q.Location, l.PlayerVC
		end
	end
end


function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, l.TopSoul, false, false
	else if n == 2
		return a.OnAttack, l.PlayerVC, l.PlayerRC, true, true
	end
	return 0
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
	end
	return false
end

function Activate(n, i)
	if n == 1 then
		obj.SuperiorCall(1)
	else if n == 2 then
		obj.AddPower(1, 2000)
		return c.BattleEnds
	end
	return 0
end

function EndCont(n)
	if n == 2 then
		obj.AddPower(1, -2000)
	end
end