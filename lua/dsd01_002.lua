-- Blaze Maiden, Reiyu

function NumberOfEffects()
	return 3
end

function NumberOfParams(n)
	return 3
end

function GetParam(n)
	if n == 1
		return q.Location, l.Deck, q.Name, "Vairina"
	else if n == 2
		return q.Count, 1
	else if n == 3
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
		if obj.IsTopSoul() and obj.VanguardIs("Chakrabarthi Divine Dragon, Nirvana") and obj.CanSB(1) then
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

function Activate(n, i)
	if n == 1 then
		obj.SoulBlast(2)
		obj.Search(1)
	else if n == 2 then
		obj.AddPower(3, 2000)
	else if n == 3 then
		obj.AddPower(3, -2000)
	end
end